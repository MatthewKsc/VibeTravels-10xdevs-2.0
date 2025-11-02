using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using VibeTravels.Infrastructure.DAL;
using VibeTravels.Infrastructure.Logging;
using VibeTravels.Infrastructure.Middlewares;
using VibeTravels.Infrastructure.Security;

namespace VibeTravels.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureBuilderConfig(this WebApplicationBuilder builder)
    {
        builder.ConfigureSerilogLogging();
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        
        services.AddSingleton<ExceptionMiddleware>();
        
        services.AddSecurity(configuration);
        services.AddHttpContextAccessor();

        services.AddPostgres(configuration);
        services.AddCommandHandlerLogging();
        // services.AddQueryHandlerLogging(); use when query will be implemented
    }

    public static void UseInfrastructure(this WebApplication application)
    {
        application.UseMiddleware<ExceptionMiddleware>();
        
        if (application.Environment.IsDevelopment())
        {
            application.MapOpenApi();
            application.MapScalarApiReference();
        }
        
        application.UseAuthentication();
        application.UseAuthorization();
    }
}