using VibeTravels.Core.Exceptions.Note;

namespace VibeTravels.Core.ValueObjects.Notes;

public sealed record NoteTitle
{
    public string Value { get; }
    
    public NoteTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new EmptyNoteTitleException();
        
        if (value.Length is < 1 or > 200)
            throw new InvalidNoteTitleRangeException();
        
        Value = value;
    }
    
    public static implicit operator string(NoteTitle title) => title.Value;
    public static implicit operator NoteTitle(string value) => new(value);
}
