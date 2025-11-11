using Microsoft.AspNetCore.Mvc;
using VibeTravels.Api.Extensions;
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
                    GetNotes query = new() { UserId = context.GetUserIdFromContext() };
                    return await handler.HandleAsync(query);
                })
            .WithName("GetNotes");
        
        builder.MapGet("/{noteId:guid}",
                async (
                    Guid noteId,
                    HttpContext context,
                    IQueryHandler<GetNote, NoteDto> handler) =>
                {
                    GetNote query = new() { UserId = context.GetUserIdFromContext(), NoteId = noteId };
                    return await handler.HandleAsync(query);
                })
            .WithName("GetNote");
        
        builder.MapPost("",
                async (
                    [FromBody] CreateNoteRequest request,
                    HttpContext context,
                    ICommandHandler<CreateNote> handler) =>
                {
                    CreateNote command = new(context.GetUserIdFromContext(), request.Title, request.Location, request.Content);
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
                    UpdateNoteDetails command = new(context.GetUserIdFromContext(), noteId, request.Title, request.Location, request.Content);
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
                    DeleteNote command = new(context.GetUserIdFromContext(), noteId);
                    await handler.HandleAsync(command);
                    return Results.NoContent();
                })
            .WithName("DeleteNote");
    }
}
