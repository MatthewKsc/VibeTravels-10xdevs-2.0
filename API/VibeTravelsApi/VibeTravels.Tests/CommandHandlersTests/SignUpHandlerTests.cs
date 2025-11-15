using AutoFixture;
using Moq;
using VibeTravels.Application.Commands.Auth;
using VibeTravels.Application.Commands.Auth.Handlers;
using VibeTravels.Application.Exceptions.Auth;
using VibeTravels.Application.Security;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Tests.CommandHandlersTests;

[TestFixture]
public class SignUpHandlerTests
{
    private Fixture fixture;
    private Mock<IUserRepository> userRepositoryMock;
    private Mock<IPasswordHasher> passwordHasherMock;
    private SignUpHandler handler;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture();
        userRepositoryMock = new Mock<IUserRepository>();
        passwordHasherMock = new Mock<IPasswordHasher>();
        handler = new SignUpHandler(userRepositoryMock.Object, passwordHasherMock.Object);
    }

    [Test]
    public async Task HandleAsync_ShouldCreateUser_WhenUserDoesNotExist()
    {
        var command = new SignUp("test@example.com", "Password123");
        userRepositoryMock
            .Setup(r => r.GetUserOrDefault(It.IsAny<Specification<User>>()))
            .ReturnsAsync((User?)null);
        passwordHasherMock
            .Setup(h => h.Hash(It.IsAny<string>()))
            .Returns(new HashedPassword("hashedPassword"));

        await handler.HandleAsync(command);

        userRepositoryMock.Verify(r => r.AddUser(It.IsAny<User>()), Times.Once);
    }

    [Test]
    public void HandleAsync_ShouldThrowUserAlreadyExistsException_WhenUserExists()
    {
        var command = new SignUp("test@example.com", "Password123");
        var existingUser = new User(
            Guid.NewGuid(),
            "test@example.com",
            new HashedPassword("hashedPassword"),
            DateTime.UtcNow,
            DateTime.UtcNow);
        
        userRepositoryMock
            .Setup(r => r.GetUserOrDefault(It.IsAny<Specification<User>>()))
            .ReturnsAsync(existingUser);

        Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await handler.HandleAsync(command));
        userRepositoryMock.Verify(r => r.AddUser(It.IsAny<User>()), Times.Never);
    }

    [Test]
    public async Task HandleAsync_ShouldHashPassword_WhenCreatingUser()
    {
        var command = new SignUp("test@example.com", "Password123");
        userRepositoryMock
            .Setup(r => r.GetUserOrDefault(It.IsAny<Specification<User>>()))
            .ReturnsAsync((User?)null);
        passwordHasherMock
            .Setup(h => h.Hash(It.IsAny<string>()))
            .Returns(new HashedPassword("hashedPassword"));

        await handler.HandleAsync(command);

        passwordHasherMock.Verify(h => h.Hash(It.IsAny<string>()), Times.Once);
    }
}

