using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Password;

public sealed class EmptyPasswordException() : VibeTravelsException("Password cannot be empty.");