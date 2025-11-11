using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Note;

public sealed class EmptyNoteTitleException() : VibeTravelsException("Note title cannot be empty");