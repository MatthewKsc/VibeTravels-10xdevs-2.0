using VibeTravels.Core.Exceptions.Profile;

namespace VibeTravels.Core.ValueObjects.Profile;

public sealed record TravelStyle
{
    public string Value { get; }
    
    //TODO: POC - TravelStyle types will be in DB and CONSTATS
    private static readonly HashSet<string> AllowedValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "Adventure", "Relaxation", "Cultural", "Luxury", "Budget", "Family", "Solo", "Romantic"
    };

    public TravelStyle(string? value)
    {
        if (value is null)
        {
            Value = null!;
            return;
        }
        
        if (!AllowedValues.Contains(value))
            throw new InvalidTravelStyleException();
        
        Value = value;
    }
    
    public static implicit operator string(TravelStyle travelStyle) => travelStyle.Value;
    public static implicit operator TravelStyle(string? value) => new(value);
}

