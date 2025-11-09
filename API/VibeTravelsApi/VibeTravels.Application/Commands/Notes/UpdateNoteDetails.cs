using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Notes;

public sealed record UpdateNoteDetails(Guid UserId, Guid NoteId, string Title, string Location, string Content) : ICommand;