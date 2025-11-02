using Humanizer;
using Microsoft.Extensions.Logging;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Infrastructure.Logging;

internal sealed class LoggingQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> inner,
    ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger) : IQueryHandler<TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    public async Task<TResult> HandleAsync(TQuery query)
    {
        string queryName = typeof(TQuery).Name.Underscore();
        string handlerName = inner.GetType().Name.Underscore();
        logger.LogInformation("Starting handling of query {Query} by {Handler}", queryName, handlerName);

        TResult result = await inner.HandleAsync(query);
        logger.LogInformation("Completed handling of query {Query}", queryName);
        
        return result;
    }
}