using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VibeTravels.Core.Repositories;
using VibeTravels.Infrastructure.DAL.Repositories;

namespace VibeTravels.Infrastructure.DAL;

internal static class Extensions
{
    private const string PostgresSqlDbConnectionSectionName = "VibeTravelsDatabase";
    
    public static void AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VibeTravelsContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString(PostgresSqlDbConnectionSectionName);

            if (connectionString is null)
                throw new InvalidOperationException("Unable to get connection string value");

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<ITripRequestRepository, TripRequestRepository>();
        services.AddScoped<IPlanGenerationRepository, PlanGenerationRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
    }
}