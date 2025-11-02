using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Application.Exceptions.Auth;

internal sealed class InvalidCredentialsException() : VibeTravelsException("Invalid credentials provided.");