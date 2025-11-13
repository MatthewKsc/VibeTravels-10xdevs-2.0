namespace VibeTravels.Application.Models.Plans;

internal sealed record InputPayloadTripRequest(int TravelDays, int Travelers, string StartDate);