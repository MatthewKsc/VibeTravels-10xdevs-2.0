using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Email;

public sealed class EmptyEmailException() : VibeTravelsException($"Provided email is null or empty");