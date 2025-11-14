using Microsoft.AspNetCore.Mvc;
using VibeTravels.Api.Extensions;
using VibeTravels.Application.Commands.Plans;
using VibeTravels.Application.DTO;
using VibeTravels.Application.DTO.Requests;
using VibeTravels.Application.Queries.Plans;
using VibeTravels.Core.Const;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Api.Endpoints;

public static class PlanEndpoints
{
    public static void MapPlanEndpoints(this RouteGroupBuilder api)
    {
        RouteGroupBuilder builder = api.MapGroup("/plans").WithTags("Plans");
        
        builder.MapGet("/{planId:guid}",
                async (
                    Guid planId,
                    HttpContext context,
                    IQueryHandler<GetPlan, PlanDto> handler) =>
                {
                    GetPlan query = new() { UserId = context.GetUserIdFromContext(), PlanId = planId };
                    return await handler.HandleAsync(query);
                })
            .WithName("GetPlan");
        
        builder.MapGet("",
                async (
                    HttpContext context,
                    IQueryHandler<GetPlans, PlanDto[]> handler) =>
                {
                    GetPlans query = new() { UserId = context.GetUserIdFromContext() };
                    return await handler.HandleAsync(query);
                })
            .WithName("GetPlans");
        
        
        builder.MapPost("",
                async (
                    [FromBody] CreatePlanRequest request,
                    HttpContext context,
                    ICommandHandler<CreatePlan> handler) =>
                {
                    CreatePlan command = new(context.GetUserIdFromContext(), request.NoteId, request.TravelDays, request.Travelers, request.StartDate);
                    await handler.HandleAsync(command);
                    return Results.Created();
                })
            .WithName("CreatePlan");
        
        builder.MapPost("/{planId:guid}/accept",
                async (
                    Guid planId,
                    [FromBody] PlanStatusDecisionRequest request,
                    HttpContext context,
                    ICommandHandler<PlanStatusDecision> handler) =>
                {
                    PlanStatusDecision command = new(context.GetUserIdFromContext(), planId, PlanStatus.Accepted, request.DecisionReason);
                    await handler.HandleAsync(command);
                    return Results.Created();
                })
            .WithName("AcceptPlan");
        
        builder.MapPost("/{planId:guid}/reject",
                async (
                    Guid planId,
                    [FromBody] PlanStatusDecisionRequest request,
                    HttpContext context,
                    ICommandHandler<PlanStatusDecision> handler) =>
                {
                    PlanStatusDecision command = new(context.GetUserIdFromContext(), planId, PlanStatus.Rejected, request.DecisionReason);
                    await handler.HandleAsync(command);
                    return Results.Created();
                })
            .WithName("RejectPlan");
        
        builder.MapPost("/{planGenerationId:guid}/retry",
                async (
                    Guid planGenerationId,
                    HttpContext context,
                    ICommandHandler<RetryPlanGeneration> handler) =>
                {
                    RetryPlanGeneration command = new(context.GetUserIdFromContext(), planGenerationId);
                    await handler.HandleAsync(command);
                    return Results.Created();
                })
            .WithName("RetryPlanGeneration");
        
        builder.MapPut("/{planId:guid}",
                async (
                    Guid planId,
                    [FromBody] UpdatePlanContentRequest request,
                    HttpContext context,
                    ICommandHandler<UpdatePlanContent> handler) =>
                {
                    UpdatePlanContent command = new(context.GetUserIdFromContext(), planId, request.ContentMd);
                    await handler.HandleAsync(command);
                    return Results.NoContent();
                })
            .WithName("UpdatePlanContent");
        
        builder.MapDelete("/{planId:guid}",
                async (
                    Guid planId,
                    HttpContext context,
                    ICommandHandler<DeletePlan> handler) =>
                {
                    DeletePlan command = new(context.GetUserIdFromContext(), planId);
                    await handler.HandleAsync(command);
                    return Results.NoContent();
                })
            .WithName("DeletePlan");
    }
}