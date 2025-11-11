using VibeTravels.Core.Exceptions;

namespace VibeTravels.Core.ValueObjects.Notes;

public sealed record NoteId
{
    public Guid Value { get; }

    public NoteId(Guid value)
    {
        if (value == Guid.Empty)
            throw new InvalidEntityIdException(value);

        Value = value;
    }

    public static implicit operator Guid(NoteId noteId) => noteId.Value;
    public static implicit operator NoteId(Guid value) => new(value);
}