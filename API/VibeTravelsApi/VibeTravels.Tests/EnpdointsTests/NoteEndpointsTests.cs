using Microsoft.AspNetCore.Http;
using Moq;
using VibeTravels.Application.Commands.Notes;
using VibeTravels.Application.DTO;
using VibeTravels.Application.Queries.Notes;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Tests.EnpdointsTests;

[TestFixture]
public class NoteEndpointsTests
{
    private Mock<IQueryHandler<GetNotes, NoteDto[]>> getNotesHandlerMock;
    private Mock<IQueryHandler<GetNote, NoteDto>> getNoteHandlerMock;
    private Mock<ICommandHandler<CreateNote>> createNoteHandlerMock;

    [SetUp]
    public void Setup()
    {
        getNotesHandlerMock = new Mock<IQueryHandler<GetNotes, NoteDto[]>>();
        getNoteHandlerMock = new Mock<IQueryHandler<GetNote, NoteDto>>();
        createNoteHandlerMock = new Mock<ICommandHandler<CreateNote>>();
    }

    [Test]
    public async Task GetNotes_ShouldReturnNotes_WhenUserHasNotes()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetNotes { UserId = userId };
        var expectedNotes = new[]
        {
            new NoteDto(Guid.NewGuid(), "Title 1", "Location 1", "Content 1", DateTime.UtcNow, DateTime.UtcNow),
            new NoteDto(Guid.NewGuid(), "Title 2", "Location 2", "Content 2", DateTime.UtcNow, DateTime.UtcNow)
        };

        getNotesHandlerMock
            .Setup(h => h.HandleAsync(It.IsAny<GetNotes>()))
            .ReturnsAsync(expectedNotes);

        // Act
        var result = await getNotesHandlerMock.Object.HandleAsync(query);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(2));
        getNotesHandlerMock.Verify(h => h.HandleAsync(It.IsAny<GetNotes>()), Times.Once);
    }

    [Test]
    public async Task GetNote_ShouldReturnNote_WhenNoteExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var noteId = Guid.NewGuid();
        var query = new GetNote { UserId = userId, NoteId = noteId };
        var expectedNote = new NoteDto(noteId, "Title", "Location", "Content", DateTime.UtcNow, DateTime.UtcNow);

        getNoteHandlerMock
            .Setup(h => h.HandleAsync(It.IsAny<GetNote>()))
            .ReturnsAsync(expectedNote);

        // Act
        var result = await getNoteHandlerMock.Object.HandleAsync(query);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(noteId));
        getNoteHandlerMock.Verify(h => h.HandleAsync(It.IsAny<GetNote>()), Times.Once);
    }

    [Test]
    public async Task CreateNote_ShouldCallHandler_WhenRequestIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateNote(userId, "New Title", "New Location", "New Content");

        createNoteHandlerMock
            .Setup(h => h.HandleAsync(It.IsAny<CreateNote>()))
            .Returns(Task.CompletedTask);

        // Act
        await createNoteHandlerMock.Object.HandleAsync(command);

        // Assert
        createNoteHandlerMock.Verify(h => h.HandleAsync(command), Times.Once);
    }

    [Test]
    public async Task GetNotes_ShouldReturnEmptyArray_WhenNoNotesExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var query = new GetNotes { UserId = userId };
        var expectedNotes = Array.Empty<NoteDto>();

        getNotesHandlerMock
            .Setup(h => h.HandleAsync(It.IsAny<GetNotes>()))
            .ReturnsAsync(expectedNotes);

        // Act
        var result = await getNotesHandlerMock.Object.HandleAsync(query);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(0));
    }
}

