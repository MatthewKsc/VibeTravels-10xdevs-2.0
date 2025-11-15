using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Notes;

public sealed record DeleteNote(Guid UserId, Guid NoteId) : ICommand;