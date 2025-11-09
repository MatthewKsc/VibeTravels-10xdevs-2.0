using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using VibeTravels.Application.Commands.Notes;
using VibeTravels.Application.DTO;
using VibeTravels.Application.DTO.Requests;
using VibeTravels.Application.Queries.Notes;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Api.Endpoints;

public static class NoteEndpoints
{
    public static void MapNoteEndpoints(this RouteGroupBuilder api)
    {
        RouteGroupBuilder builder = api.MapGroup("/notes").WithTags("Notes");
        
        builder.MapGet("",
                async (
                    HttpContext context,
                    IQueryHandler<GetNotes, NoteDto[]> handler) =>
                {
                    GetNotes query = new() { UserId = GetUserIdFromContext(context) };
                    return await handler.HandleAsync(query);
                })
            .WithName("GetNotes");
        
        builder.MapGet("/{noteId:guid}",
                async (
                    Guid noteId,
                    HttpContext context,
                    IQueryHandler<GetNote, NoteDto> handler) =>
                {
                    GetNote query = new() { UserId = GetUserIdFromContext(context), NoteId = noteId };
                    return await handler.HandleAsync(query);
                })
            .WithName("GetNote");
        
        builder.MapPost("",
                async (
                    [FromBody] CreateNoteRequest request,
                    HttpContext context,
                    ICommandHandler<CreateNote> handler) =>
                {
                    CreateNote command = new(GetUserIdFromContext(context), request.Title, request.Location, request.Content);
                    await handler.HandleAsync(command);
                    return Results.Created();
                })
            .WithName("CreateNote");
        
        builder.MapPut("/{noteId:guid}",
                async (
                    Guid noteId,
                    [FromBody] UpdateNoteRequest request,
                    HttpContext context,
                    ICommandHandler<UpdateNoteDetails> handler) =>
                {
                    UpdateNoteDetails command = new(GetUserIdFromContext(context), noteId, request.Title, request.Location, request.Content);
                    await handler.HandleAsync(command);
                    return Results.NoContent();
                })
            .WithName("UpdateNote");
        
        builder.MapDelete("/{noteId:guid}",
                async (
                    Guid noteId,
                    HttpContext context,
                    ICommandHandler<DeleteNote> handler) =>
                {
                    DeleteNote command = new(GetUserIdFromContext(context), noteId);
                    await handler.HandleAsync(command);
                    return Results.NoContent();
                })
            .WithName("DeleteNote");
    }
    
    private static Guid GetUserIdFromContext(HttpContext context)
    {
        string? userIdClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                              ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token.");
        }
        
        return userId;
    }
}
