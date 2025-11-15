using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Password;

public sealed class WeakPasswordException() : VibeTravelsException("Password is too weak.");