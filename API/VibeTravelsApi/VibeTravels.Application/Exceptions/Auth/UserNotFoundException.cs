using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Auth;

internal sealed class UserNotFoundException(string email)
    : VibeTravelsException($"User with email '{email}' was not found.");