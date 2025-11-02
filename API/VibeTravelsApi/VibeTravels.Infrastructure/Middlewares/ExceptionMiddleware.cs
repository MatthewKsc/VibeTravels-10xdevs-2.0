using Humanizer;
using Microsoft.AspNetCore.Http;
using VibeTravels.Infrastructure.Models;
using VibeTravels.Shared.Exceptions;

namespace VibeTravels.Infrastructure.Middlewares;

internal sealed class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(exception, context);
        }
    }
    
    private async Task HandleExceptionAsync(Exception exception, HttpContext context)
    {
        (int statusCode, Error error) = exception switch
        {
            VibeTravelsException => (StatusCodes.Status400BadRequest,
                new Error(exception.GetType().Name.Underscore().Replace("_exception", string.Empty), exception.Message)),
            _ => (StatusCodes.Status500InternalServerError, new Error("unexpected_error", "An unexpected error occurred."))
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(error);
    }
}