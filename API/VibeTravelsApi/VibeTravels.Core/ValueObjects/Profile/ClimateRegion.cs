using VibeTravels.Core.Exceptions.Profile;

namespace VibeTravels.Core.ValueObjects.Profile;

public sealed record ClimateRegion
{
    public string Value { get; }
    
    //TODO: MVP - ClimateRegion types will be in DB and CONSTATS
    private static readonly HashSet<string> AllowedValues = new(StringComparer.OrdinalIgnoreCase)
    {
        "Tropical", "Arid", "Temperate", "Continental", "Polar", "Mediterranean", "Subtropical"
    };

    public ClimateRegion(string? value)
    {
        if (value is null)
        {
            Value = null!;
            return;
        }
        
        if (!AllowedValues.Contains(value))
            throw new InvalidClimateRegionException();
        
        Value = value;
    }
    
    public static implicit operator string(ClimateRegion climateRegion) => climateRegion.Value;
    public static implicit operator ClimateRegion(string? value) => new(value ?? string.Empty);
}