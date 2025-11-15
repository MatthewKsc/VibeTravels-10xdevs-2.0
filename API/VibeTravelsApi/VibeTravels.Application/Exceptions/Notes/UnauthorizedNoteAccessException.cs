using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Notes;

internal sealed class UnauthorizedNoteAccessException(Guid noteId, Guid userId)
    : VibeTravelsException($"User '{userId}' is not authorized to access note '{noteId}'.");