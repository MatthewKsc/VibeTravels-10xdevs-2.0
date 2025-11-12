using VibeTravels.Core.Exceptions;

namespace VibeTravels.Core.ValueObjects.PlanGenerations;

public sealed record PlanGenerationId
{
    public Guid Value { get; }

    public PlanGenerationId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidEntityIdException(value);

        Value = value;
    }

    public static implicit operator Guid(PlanGenerationId planGenerationId) => planGenerationId.Value;
    public static implicit operator PlanGenerationId(Guid value) => new(value);
}

