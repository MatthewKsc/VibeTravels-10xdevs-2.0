using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Core.Exceptions.Profile;

public sealed class InvalidThemeException() : VibeTravelsException("Invalid theme. Allowed values: light, dark");