using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using VibeTravels.Infrastructure.DAL;
using VibeTravels.Infrastructure.Security;

namespace VibeTravels.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        
        services.AddSecurity(configuration);
        services.AddHttpContextAccessor();

        services.AddPostgres(configuration);
    }

    public static void UseInfrastructure(this WebApplication application)
    {
        application.UseAuthentication();
        application.UseAuthorization();
        
        if (application.Environment.IsDevelopment())
        {
            application.MapOpenApi();
            application.MapScalarApiReference();
        }

        application.UseHttpsRedirection();
    }
}