using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Email;

public sealed class InvalidEmailFormatException() : VibeTravelsException("The provided email has invalid format");