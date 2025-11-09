using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Note;

public sealed class EmptyNoteBodyException() : VibeTravelsException("Note body cannot be empty");

