namespace VibeTravels.Application.DTO;

public sealed record PlanDto(
    Guid Id,
    Guid PlanGenerationId,
    int Travelers,
    int TravelDays,
    DateTime StartDate,
    string StructureType,
    string? GenerationStatus,
    string DecisionStatus,
    string? ContentMd,
    string? ErrorMessage,
    DateTime LastUpdatedAt);