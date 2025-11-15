using AutoFixture;
using Moq;
using VibeTravels.Application.Queries.Notes;
using VibeTravels.Application.Queries.Notes.Handlers;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Tests.QueryHandlersTests;

[TestFixture]
public class GetNotesHandlerTests
{
    private Fixture fixture;
    private Mock<INoteRepository> noteRepositoryMock;
    private GetNotesHandler handler;

    [SetUp]
    public void Setup()
    {
        fixture = new Fixture();
        noteRepositoryMock = new Mock<INoteRepository>();
        handler = new GetNotesHandler(noteRepositoryMock.Object);
    }

    [Test]
    public async Task HandleAsync_ShouldReturnNotes_WhenNotesExist()
    {
        var userId = Guid.NewGuid();
        var query = new GetNotes { UserId = userId };
        var notes = new List<Note>
        {
            new(Guid.NewGuid(), userId, new NoteTitle("Title 1"), new NoteLocation("Location 1"), 
                new NoteBody("Exploring the Big Apple! Want to see Central Park, Times Square, and Broadway shows. Looking for the best pizza and bagels in town."), DateTime.UtcNow, DateTime.UtcNow),
            new(Guid.NewGuid(), userId, new NoteTitle("Title 2"), new NoteLocation("Location 2"), 
                new NoteBody("Planning a trip to Tokyo to experience traditional and modern Japan. Interested in temples, technology districts, and amazing food scene."), DateTime.UtcNow, DateTime.UtcNow)
        };

        noteRepositoryMock
            .Setup(r => r.GetNotes(It.IsAny<Specification<Note>>()))
            .ReturnsAsync(notes);

        var result = await handler.HandleAsync(query);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(2));
    }

    [Test]
    public async Task HandleAsync_ShouldReturnEmptyArray_WhenNoNotesExist()
    {
        var userId = Guid.NewGuid();
        var query = new GetNotes { UserId = userId };
        var notes = new List<Note>();

        noteRepositoryMock
            .Setup(r => r.GetNotes(It.IsAny<Specification<Note>>()))
            .ReturnsAsync(notes);

        var result = await handler.HandleAsync(query);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }

    [Test]
    public async Task HandleAsync_ShouldMapNotesToDtos_Correctly()
    {
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var query = new GetNotes { UserId = userId };
        var notes = new List<Note>
        {
            new(
                noteId, 
                userId,
                new NoteTitle("Tokyo Journey"),
                new NoteLocation("Tokyo, Japan"), 
                new NoteBody("Planning a trip to Tokyo to experience traditional and modern Japan. Interested in temples, technology districts, and amazing food scene. "), DateTime.UtcNow, DateTime.UtcNow)
        };

        noteRepositoryMock
            .Setup(r => r.GetNotes(It.IsAny<Specification<Note>>()))
            .ReturnsAsync(notes);

        var result = await handler.HandleAsync(query);

        Assert.That(result[0].Id, Is.EqualTo(noteId));
        Assert.That(result[0].Title, Is.EqualTo("Tokyo Journey"));
        Assert.That(result[0].Location, Is.EqualTo("Tokyo, Japan"));
        Assert.That(result[0].Content, Is.Not.Empty);
    }

    [Test]
    public async Task HandleAsync_ShouldCallRepository_WithCorrectSpecification()
    {
        var userId = Guid.NewGuid();
        var query = new GetNotes { UserId = userId };
        var notes = new List<Note>();

        noteRepositoryMock
            .Setup(r => r.GetNotes(It.IsAny<Specification<Note>>()))
            .ReturnsAsync(notes);

        await handler.HandleAsync(query);

        noteRepositoryMock.Verify(r => r.GetNotes(It.IsAny<Specification<Note>>()), Times.Once);
    }
}

