using VibeTravels.Application.Exceptions.Notes;
using VibeTravels.Application.Specifications.Notes;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Commands.Notes.Handlers;

public sealed class UpdateNoteDetailsHandler(INoteRepository noteRepository) : ICommandHandler<UpdateNoteDetails>
{
    public async Task HandleAsync(UpdateNoteDetails command)
    {
        UserId userId = command.UserId;
        NoteId noteId = command.NoteId;
        NoteTitle title = command.Title;
        NoteLocation location = command.Location;
        NoteBody body = command.Content;

        Specification<Note> specification = new NoteIdSpecification(noteId)
            .And(new NoteUserIdSpecification(userId));
        
        Note? userNoteExists = await noteRepository.GetNoteOrDefault(specification);

        if (userNoteExists is null)
        {
            Note? noteExists = await noteRepository.GetNoteOrDefault(new NoteIdSpecification(noteId));
            if (noteExists is null)
                throw new NoteNotFoundException(noteId);
            
            throw new UnauthorizedNoteAccessException(noteId, userId);
        }

        userNoteExists.UpdateDetails(title, location, body, DateTime.UtcNow);
        await noteRepository.UpdateNote(userNoteExists);
    }
}