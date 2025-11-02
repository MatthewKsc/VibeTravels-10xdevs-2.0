using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands;

public sealed record SignIn(string Email, string Password) : ICommand;