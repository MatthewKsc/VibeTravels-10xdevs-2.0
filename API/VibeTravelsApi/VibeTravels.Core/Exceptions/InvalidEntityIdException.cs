using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions;

public sealed class InvalidEntityIdException(Guid id) : VibeTravelsException($"The provided entity ID is invalid: {id}");