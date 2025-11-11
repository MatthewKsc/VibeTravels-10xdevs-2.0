using VibeTravels.Core.Exceptions.Note;

namespace VibeTravels.Core.ValueObjects.Notes;

public sealed record NoteBody
{
    public string Value { get; }
    
    public NoteBody(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyNoteBodyException();
        
        if (value.Length is < 100 or > 10000)
            throw new InvalidNoteBodyRangeException();
        
        Value = value;
    }
    
    public static implicit operator string(NoteBody body) => body.Value;
    public static implicit operator NoteBody(string value) => new(value);
}

