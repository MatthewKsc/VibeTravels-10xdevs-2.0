using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Plans;

public sealed record CreatePlan(Guid UserId, Guid NoteId, int TravelDays, int Travelers, DateTime StartDate) : ICommand;