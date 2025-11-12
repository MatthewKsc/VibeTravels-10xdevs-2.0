using VibeTravels.Core.Exceptions.TripRequest;

namespace VibeTravels.Core.ValueObjects.TripRequests;

public sealed record Travelers
{
    private const int MaxTravelers = 20;
    
    public int Value { get; }
    
    public Travelers(int value)
    {
        if (value <= 0)
            throw new InvalidTravelersException();

        if (value > MaxTravelers)
            throw new InvalidTravelersRangeException(MaxTravelers);
                
        Value = value;
    }
    
    public static implicit operator int(Travelers travelers) => travelers.Value;
    public static implicit operator Travelers(int value) => new(value);
}