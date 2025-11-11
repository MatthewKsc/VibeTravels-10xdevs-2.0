using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Notes;

public sealed record CreateNote(Guid UserId, string Title, string Location, string Content) : ICommand;