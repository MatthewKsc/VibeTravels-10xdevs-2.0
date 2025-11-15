using Humanizer;
using Microsoft.Extensions.Logging;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Infrastructure.Logging;

internal sealed class LoggingCommandHandlerDecorator<TCommand>(
    ICommandHandler<TCommand> commandHandler,
    ILogger<LoggingCommandHandlerDecorator<TCommand>> logger) : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command)
    {
        string commandName = typeof(TCommand).Name.Underscore();
        string handlerName = commandHandler.GetType().Name.Underscore();
        logger.LogInformation("Starting handling of command {Command} by {Handler}", commandName, handlerName);
        
        await commandHandler.HandleAsync(command);
        logger.LogInformation("Completed handling of command {Command}", commandName);
    }
}