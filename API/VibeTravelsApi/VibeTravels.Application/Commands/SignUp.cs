using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands;

public sealed record SignUp(string Email, string Password) : ICommand;