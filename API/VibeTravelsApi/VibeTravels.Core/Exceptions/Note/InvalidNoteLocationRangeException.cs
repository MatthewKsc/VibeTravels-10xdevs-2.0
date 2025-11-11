using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Note;

public sealed class InvalidNoteLocationRangeException() : VibeTravelsException("Note location must be between 1 and 255 characters");

