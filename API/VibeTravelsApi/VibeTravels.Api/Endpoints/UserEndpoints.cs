using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VibeTravels.Application.Commands;
using VibeTravels.Application.Commands.Auth;
using VibeTravels.Application.DTO;
using VibeTravels.Application.Security;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this RouteGroupBuilder api)
    {
        RouteGroupBuilder builder = api.MapGroup("/users").WithTags("Users");
        
        builder.MapPost("/signup",
            async (
                [FromBody] SignUp command,
                ICommandHandler<SignUp> handler) => await handler.HandleAsync(command))
            .WithName("SignUpUser");
        
        builder.MapPost("/signin", async ([FromBody] SignIn command, ICommandHandler<SignIn> handler, ITokenStorage tokenStorage) =>
            {
                await handler.HandleAsync(command);
                JwtDto? jwt = tokenStorage.RetrieveToken();
                return jwt is not null ? Results.Ok(jwt) : Results.Unauthorized();
            })
            .WithName("SignInUser");
        
        //TODO: Temporary endpoint to check API authorization, remove it later
        builder.MapGet("/alive", [Authorize] () => Results.Ok("Api Alive"))
            .WithName("Alive")
            .RequireAuthorization();
    }
}