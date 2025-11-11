using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Notes;

internal sealed class NoteNotFoundException(Guid noteId)
    : VibeTravelsException($"Note with ID '{noteId}' was not found.");