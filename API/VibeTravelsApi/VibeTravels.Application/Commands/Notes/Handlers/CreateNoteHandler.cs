using VibeTravels.Application.Exceptions.Auth;
using VibeTravels.Application.Specifications.Users;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Commands.Notes.Handlers;

public sealed class CreateNoteHandler(
    INoteRepository noteRepository,
    IUserRepository userRepository) : ICommandHandler<CreateNote>
{
    public async Task HandleAsync(CreateNote command)
    {
        UserId userId = command.UserId;
        NoteTitle title = command.Title;
        NoteLocation location = command.Location;
        NoteBody body = command.Content;

        User? existingUser = await userRepository.GetUserOrDefault(new UserIdSpecification(userId));
        
        if (existingUser is null)
            throw new UserNotFoundException(userId.Value.ToString());

        Note note = new(
            Guid.NewGuid(),
            userId,
            title,
            location,
            body,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await noteRepository.AddNote(note);
    }
}