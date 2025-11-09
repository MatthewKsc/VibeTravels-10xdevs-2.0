using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Auth;

public sealed record SignIn(string Email, string Password) : ICommand;