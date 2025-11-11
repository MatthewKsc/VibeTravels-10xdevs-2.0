using Microsoft.AspNetCore.Mvc;
using VibeTravels.Api.Extensions;
using VibeTravels.Application.Commands.Profile;
using VibeTravels.Application.DTO;
using VibeTravels.Application.DTO.Requests;
using VibeTravels.Application.Queries.Profile;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Api.Endpoints;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this RouteGroupBuilder api)
    {
        RouteGroupBuilder builder = api.MapGroup("/profiles").WithTags("Profiles");
        
        builder.MapGet("/me",
                async (
                    HttpContext context,
                    IQueryHandler<GetProfile, ProfileDto?> handler) =>
                {
                    GetProfile query = new() { UserId = context.GetUserIdFromContext() };
                    return await handler.HandleAsync(query);
                })
            .WithName("GetProfile");
        
        builder.MapPut("/me",
                async (
                    [FromBody] UpdateProfileRequest request,
                    HttpContext context,
                    ICommandHandler<UpdateProfile> handler) =>
                {
                    UpdateProfile command = new(context.GetUserIdFromContext(), request.TravelStyle, request.AccommodationPreference,
                        request.ClimatePreference);
                    await handler.HandleAsync(command);
                    return Results.Created();
                })
            .WithName("UpdateProfile");
    }
}