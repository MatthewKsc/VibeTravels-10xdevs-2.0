namespace VibeTravels.Application.DTO.Requests;

public sealed record UpdateNoteRequest(string Title, string Location, string Content);