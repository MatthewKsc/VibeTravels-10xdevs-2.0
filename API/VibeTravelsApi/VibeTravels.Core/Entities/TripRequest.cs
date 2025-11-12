using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.TripRequests;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Core.Entities;

public sealed class TripRequest
{
    public TripRequestId Id { get; private set; }
    public UserId UserId { get; private set; }
    public NoteId NoteId { get; private set; }
    public TravelDays Days { get; private set; }
    public Travelers Travelers { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public User User { get; private set; }
    public Note Note { get; private set; }

    public TripRequest(
        TripRequestId id,
        UserId userId,
        NoteId noteId,
        TravelDays days,
        Travelers travelers,
        DateTime startDate,
        DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        NoteId = noteId;
        Days = days;
        Travelers = travelers;
        StartDate = startDate;
        CreatedAt = createdAt;
    }

    public void UpdateDetails(TravelDays days, Travelers travelers, DateTime startDate)
    {
        Days = days;
        Travelers = travelers;
        StartDate = startDate;
    }
}