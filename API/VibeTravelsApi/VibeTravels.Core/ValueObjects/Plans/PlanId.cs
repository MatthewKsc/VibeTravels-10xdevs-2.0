using VibeTravels.Core.Exceptions;

namespace VibeTravels.Core.ValueObjects.Plans;

public sealed record PlanId
{
    public Guid Value { get; }

    public PlanId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidEntityIdException(value);

        Value = value;
    }

    public static implicit operator Guid(PlanId planId) => planId.Value;
    public static implicit operator PlanId(Guid value) => new(value);
}

