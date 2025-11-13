using VibeTravels.Core.Exceptions;

namespace VibeTravels.Core.ValueObjects.TripRequests;

public sealed record TripRequestId
{
    public Guid Value { get; }

    public TripRequestId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidEntityIdException(value);

        Value = value;
    }

    public static implicit operator Guid(TripRequestId tripRequestId) => tripRequestId.Value;
    public static implicit operator TripRequestId(Guid value) => new(value);
}