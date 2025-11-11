namespace VibeTravels.Application.DTO;

public sealed record NoteDto(
    Guid Id,
    string Title,
    string Location,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt);