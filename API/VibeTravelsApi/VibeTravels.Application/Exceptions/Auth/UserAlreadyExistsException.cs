using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Auth;

internal sealed class UserAlreadyExistsException(string email)
    : VibeTravelsException($"User with email '{email}' already exists.");