using VibeTravels.Core.Exceptions.Note;

namespace VibeTravels.Core.ValueObjects.Notes;

public sealed record NoteLocation
{
    public string Value { get; }
    
    public NoteLocation(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyNoteLocationException();
        
        if (value.Length is < 1 or > 255)
            throw new InvalidNoteLocationRangeException();
        
        Value = value;
    }
    
    public static implicit operator string(NoteLocation location) => location.Value;
    public static implicit operator NoteLocation(string value) => new(value);
}