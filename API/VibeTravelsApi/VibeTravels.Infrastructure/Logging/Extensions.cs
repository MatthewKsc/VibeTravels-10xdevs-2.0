using Microsoft.Extensions.DependencyInjection;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Infrastructure.Logging;

internal static class Extensions
{
    public static void AddCommandHandlerLogging(this IServiceCollection services) =>
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));

    public static void AddQueryHandlerLogging(this IServiceCollection services) =>
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
}