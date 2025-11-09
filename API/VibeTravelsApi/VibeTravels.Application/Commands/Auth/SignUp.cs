using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Auth;

public sealed record SignUp(string Email, string Password) : ICommand;