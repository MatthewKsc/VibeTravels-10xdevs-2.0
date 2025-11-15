using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Note;

public sealed class EmptyNoteLocationException() : VibeTravelsException("Note location cannot be empty");

