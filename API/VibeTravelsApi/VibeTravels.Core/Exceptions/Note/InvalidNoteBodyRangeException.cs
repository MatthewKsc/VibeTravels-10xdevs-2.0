using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Note;

public sealed class InvalidNoteBodyRangeException() : VibeTravelsException("Note body must be between 1000 and 10000 characters");

