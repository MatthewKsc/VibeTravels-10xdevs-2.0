using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Email;

public sealed class EmailToLongException() : VibeTravelsException($"The provided email is too long");