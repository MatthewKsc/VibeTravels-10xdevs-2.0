using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VibeTravels.Infrastructure.Cors;

internal static class CorsExtensions
{
    public static IServiceCollection AddCorsSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptions = configuration.GetSection("Cors").Get<Options.CorsOptions>();

        if (corsOptions is null || corsOptions.AllowedOrigins.Length == 0)
            throw new ArgumentException("Cors options cannot be empty");

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(corsOptions?.AllowedOrigins ?? [])
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });

        return services;
    }
}