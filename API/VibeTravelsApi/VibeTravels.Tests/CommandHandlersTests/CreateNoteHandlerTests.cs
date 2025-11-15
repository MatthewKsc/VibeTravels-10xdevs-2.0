using AutoFixture;
using Moq;
using VibeTravels.Application.Commands.Notes;
using VibeTravels.Application.Commands.Notes.Handlers;
using VibeTravels.Application.Exceptions.Auth;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Tests.CommandHandlersTests;

[TestFixture]
public class CreateNoteHandlerTests
{
    private Fixture fixture;
    private Mock<INoteRepository> noteRepositoryMock;
    private Mock<IUserRepository> userRepositoryMock;
    private CreateNoteHandler handler;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture();
        noteRepositoryMock = new Mock<INoteRepository>();
        userRepositoryMock = new Mock<IUserRepository>();
        handler = new CreateNoteHandler(noteRepositoryMock.Object, userRepositoryMock.Object);
    }

    [Test]
    public async Task HandleAsync_ShouldCreateNote_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var command = new CreateNote(userId,
            "New York City",
            "New York, USA",
            "Exploring the Big Apple! Want to see Central Park, Times Square, and Broadway shows. Looking for the best pizza and bagels in town.");
        var existingUser = new User(
            userId,
            "test@example.com",
            new HashedPassword("hashedPassword"),
            DateTime.UtcNow,
            DateTime.UtcNow);

        userRepositoryMock
            .Setup(r => r.GetUserOrDefault(It.IsAny<Specification<User>>()))
            .ReturnsAsync(existingUser);

        await handler.HandleAsync(command);

        noteRepositoryMock.Verify(r => r.AddNote(It.IsAny<Note>()), Times.Once);
    }

    [Test]
    public void HandleAsync_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        var command = new CreateNote(userId,
            "New York City",
            "New York, USA",
            "Exploring the Big Apple! Want to see Central Park, Times Square, and Broadway shows. Looking for the best pizza and bagels in town.");
        
        userRepositoryMock
            .Setup(r => r.GetUserOrDefault(It.IsAny<Specification<User>>()))
            .ReturnsAsync((User?)null);

        Assert.ThrowsAsync<UserNotFoundException>(async () => await handler.HandleAsync(command));
        noteRepositoryMock.Verify(r => r.AddNote(It.IsAny<Note>()), Times.Never);
    }

    [Test]
    public async Task HandleAsync_ShouldCheckUserExistence_BeforeCreatingNote()
    {
        var userId = Guid.NewGuid();
        var command = new CreateNote(userId,
            "New York City",
            "New York, USA",
            "Exploring the Big Apple! Want to see Central Park, Times Square, and Broadway shows. Looking for the best pizza and bagels in town.");
        var existingUser = new User(
            userId,
            "test@example.com",
            new HashedPassword("hashedPassword"),
            DateTime.UtcNow,
            DateTime.UtcNow);

        userRepositoryMock
            .Setup(r => r.GetUserOrDefault(It.IsAny<Specification<User>>()))
            .ReturnsAsync(existingUser);

        await handler.HandleAsync(command);

        userRepositoryMock.Verify(r => r.GetUserOrDefault(It.IsAny<Specification<User>>()), Times.Once);
    }
}

