using Microsoft.AspNetCore.Authorization;

namespace VibeTravels.Api.Endpoints;

public static class SystemEndpoints
{
    public static void MapSystemEndpoints(this RouteGroupBuilder api)
    {
        api.MapGet("/health", [AllowAnonymous] () => Results.Ok("Api Alive"))
            .WithName("HealthCheck")
            .AllowAnonymous();
    }
}

