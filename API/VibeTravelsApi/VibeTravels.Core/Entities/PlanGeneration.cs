using VibeTravels.Core.Const;
using VibeTravels.Core.ValueObjects.PlanGenerations;
using VibeTravels.Core.ValueObjects.TripRequests;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Core.Entities;

public sealed class PlanGeneration
{
    public PlanGenerationId Id { get; private set; }
    public UserId UserId { get; private set; }
    public TripRequestId TripRequestId { get; private set; }
    public PlanGenerationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? FinishedAt { get; private set; }
    public string InputPayload { get; private set; }
    public string? OutputRaw { get; private set; }
    public string? ErrorMessage { get; private set; }
    public User User { get; private set; }
    public TripRequest TripRequest { get; private set; }

    public PlanGeneration(
        PlanGenerationId id,
        UserId userId,
        TripRequestId tripRequestId,
        PlanGenerationStatus status,
        DateTime createdAt,
        string inputPayload)
    {
        Id = id;
        UserId = userId;
        TripRequestId = tripRequestId;
        Status = status;
        CreatedAt = createdAt;
        InputPayload = inputPayload;
    }

    public void MarkAsStarted(DateTime startedAt)
    {
        Status = PlanGenerationStatus.Running;
        StartedAt = startedAt;
    }

    public void MarkAsSucceeded(DateTime finishedAt, string outputRaw)
    {
        Status = PlanGenerationStatus.Succeeded;
        FinishedAt = finishedAt;
        OutputRaw = outputRaw;
    }

    public void MarkAsFailed(DateTime finishedAt, string errorMessage)
    {
        Status = PlanGenerationStatus.Failed;
        FinishedAt = finishedAt;
        ErrorMessage = errorMessage;
    }

    public void ResetToQueued()
    {
        Status = PlanGenerationStatus.Queued;
        StartedAt = null;
        FinishedAt = null;
        OutputRaw = null;
        ErrorMessage = null;
    }
}

