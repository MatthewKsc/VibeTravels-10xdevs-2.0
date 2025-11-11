using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Note;

public sealed class InvalidNoteTitleRangeException() : VibeTravelsException("Note title must be between 1 and 200 characters");

