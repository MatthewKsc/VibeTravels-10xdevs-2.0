using VibeTravels.Core.Exceptions;

namespace VibeTravels.Core.ValueObjects.Profile;

public sealed record ProfileId
{
    public Guid Value { get; }

    public ProfileId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidEntityIdException(value);

        Value = value;
    }

    public static implicit operator Guid(ProfileId profileId) => profileId.Value;
    public static implicit operator ProfileId(Guid value) => new(value);
}