using VibeTravels.Core.Exceptions.TripRequest;

namespace VibeTravels.Core.ValueObjects.TripRequests;

public sealed record TravelDays
{
    private const int MaxTravelDays = 60;
    
    public int Value { get; }
    
    public TravelDays(int value)
    {
        if (value <= 0)
            throw new InvalidTravelDaysException();
        
        if (value > MaxTravelDays)
            throw new InvalidTravelDaysRangeException(MaxTravelDays);

        Value = value;
    }
    
    public static implicit operator int(TravelDays travelDays) => travelDays.Value;
    public static implicit operator TravelDays(int value) => new(value);
}