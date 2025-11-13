using VibeTravels.Core.Const;
using VibeTravels.Core.ValueObjects.PlanGenerations;
using VibeTravels.Core.ValueObjects.Plans;
using VibeTravels.Core.ValueObjects.TripRequests;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Core.Entities;

public sealed class Plan
{
    public PlanId Id { get; private set; }
    public UserId UserId { get; private set; }
    public TripRequestId TripRequestId { get; private set; }
    public PlanGenerationId PlanGenerationId { get; private set; }
    public PlanStructure StructureType { get; private set; }
    public int? DaysCount { get; private set; }
    public string Content { get; private set; }
    public PlanStatus Status { get; private set; }
    public string? DecisionReason { get; private set; }
    public DateTime? DecisionAt { get; private set; }
    public bool AdjustedByUser { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public TripRequest TripRequest { get; private set; }
    public PlanGeneration PlanGeneration { get; private set; }
    

    public Plan(
        PlanId id,
        UserId userId,
        TripRequestId tripRequestId,
        PlanGenerationId planGenerationId,
        PlanStructure structureType,
        int? daysCount,
        string content,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        UserId = userId;
        TripRequestId = tripRequestId;
        PlanGenerationId = planGenerationId;
        StructureType = structureType;
        DaysCount = daysCount;
        Content = content;
        Status = PlanStatus.Generated;
        AdjustedByUser = false;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void UpdateContent(string content, DateTime updatedAt)
    {
        Content = content;
        AdjustedByUser = true;
        UpdatedAt = updatedAt;
    }

    public void Accept(DateTime decisionAt, string? reason = null)
    {
        Status = PlanStatus.Accepted;
        DecisionAt = decisionAt;
        DecisionReason = reason;
        UpdatedAt = decisionAt;
    }

    public void Reject(DateTime decisionAt, string? reason = null)
    {
        Status = PlanStatus.Rejected;
        DecisionAt = decisionAt;
        DecisionReason = reason;
        UpdatedAt = decisionAt;
    }

    public void ResetForRetry(DateTime updatedAt)
    {
        Status = PlanStatus.NotGenerated;
        DecisionAt = null;
        DecisionReason = null;
        AdjustedByUser = false;
        UpdatedAt = updatedAt;
    }
}

