using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Core.Entities;

public sealed class Note
{
    public NoteId Id { get; private set; }
    public UserId UserId { get; private set; }
    public NoteTitle Title { get; private set; }
    public NoteLocation Location { get; private set; }
    public NoteBody Body { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public Note(
        NoteId id,
        UserId userId,
        NoteTitle title,
        NoteLocation location,
        NoteBody body,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        UserId = userId;
        Title = title;
        Location = location;
        Body = body;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    public void MarkAsDeleted(DateTime deletedAt) => DeletedAt = deletedAt;
}