using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Password;

public sealed class InvalidPasswordRangeException() : VibeTravelsException("Password does not meet the required length constraints.");