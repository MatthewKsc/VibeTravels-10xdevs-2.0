namespace VibeTravels.Application.DTO.Requests;

public sealed record CreatePlanRequest(Guid NoteId, int TravelDays, int Travelers, DateTime StartDate);