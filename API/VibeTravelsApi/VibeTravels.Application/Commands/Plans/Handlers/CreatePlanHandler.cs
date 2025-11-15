using System.Text.Json;
using VibeTravels.Application.Exceptions.Auth;
using VibeTravels.Application.Exceptions.Notes;
using VibeTravels.Application.Models.Plans;
using VibeTravels.Application.Specifications.Notes;
using VibeTravels.Application.Specifications.Profile;
using VibeTravels.Application.Specifications.Users;
using VibeTravels.Core.Const;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using ProfileEntity = VibeTravels.Core.Entities.Profile;

namespace VibeTravels.Application.Commands.Plans.Handlers;

public sealed class CreatePlanHandler(
    IUserRepository userRepository,
    INoteRepository noteRepository,
    IProfileRepository profileRepository,
    ITripRequestRepository tripRequestRepository,
    IPlanGenerationRepository planGenerationRepository) : ICommandHandler<CreatePlan>
{
    public async Task HandleAsync(CreatePlan command)
    {
        UserId userId = command.UserId;
        NoteId noteId = command.NoteId;
        
        User? user = await userRepository.GetUserOrDefault(new UserIdSpecification(userId));
        if (user is null)
            throw new UserNotFoundException(command.UserId.ToString());
        
        Note? note = await noteRepository.GetNoteOrDefault(new NoteIdSpecification(noteId));
        if (note is null)
            throw new NoteNotFoundException(noteId.Value);
        
        if (note.UserId != userId)
            throw new UnauthorizedNoteAccessException(noteId.Value, userId.Value);
        
        ProfileEntity? userProfile = await profileRepository.GetProfileOrDefault(new ProfileUserIdSpecification(userId));
        
        TripRequest tripRequest = new(
            Guid.NewGuid(),
            userId,
            noteId,
            command.TravelDays,
            command.Travelers,
            command.StartDate,
            DateTime.UtcNow
        );
        
        object inputPayload = BuildInputPayload(tripRequest, note, userProfile);
        string inputPayloadJson = JsonSerializer.Serialize(inputPayload, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        
        string planTitle = $"{note.Title.Value} - Daily";
        
        PlanGeneration planGeneration = new(
            Guid.NewGuid(),
            userId,
            tripRequest.Id,
            planTitle,
            PlanGenerationStatus.Queued,
            DateTime.UtcNow,
            inputPayloadJson
        );
        
        await tripRequestRepository.AddTripRequest(tripRequest);
        await planGenerationRepository.AddPlanGeneration(planGeneration);
    }
    
    private static PlanInputPayload BuildInputPayload(TripRequest tripRequest, Note note, ProfileEntity? profile) =>
        new(
            TripRequest: new InputPayloadTripRequest(
                tripRequest.Days.Value,
                tripRequest.Travelers.Value,
                tripRequest.StartDate.ToString("yyyy-MM-dd")),
            Note: new InputPayloadNote(note.Title.Value, note.Body.Value, note.Location.Value),
            Profile: new InputPayloadPreferences(
                TravelStyle: profile?.TravelStyle is not null ? profile.TravelStyle.ToString() : "No Preference",
                AccommodationType: profile?.AccommodationType is not null ? profile.AccommodationType.ToString() : "No Preference",
                ClimateRegion: profile?.ClimateRegion is not null ? profile.ClimateRegion.ToString() : "No Preference")
        );
}