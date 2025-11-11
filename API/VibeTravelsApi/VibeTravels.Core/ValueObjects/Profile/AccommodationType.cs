using VibeTravels.Core.Exceptions.Profile;

namespace VibeTravels.Core.ValueObjects.Profile;

public sealed record AccommodationType
{
    public string Value { get; }
    
    //TODO: MVP - AccommodationType types will be in DB and CONSTATS
    private static readonly HashSet<string> AllowedValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "Hotel", "Hostel", "Resort", "Apartment", "Villa", "Camping", "BedAndBreakfast", "Guesthouse"
    };

    public AccommodationType(string? value)
    {
        if (value is null)
        {
            Value = null!;
            return;
        }
        
        if (!AllowedValues.Contains(value))
            throw new InvalidAccommodationTypeException();
        
        Value = value;
    }
    
    public static implicit operator string(AccommodationType climateRegion) => climateRegion.Value;
    public static implicit operator AccommodationType(string? value) => new(value ?? string.Empty);
}