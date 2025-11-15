namespace VibeTravels.Application.DTO.Requests;

public sealed record CreateNoteRequest(string Title, string Location, string Content);