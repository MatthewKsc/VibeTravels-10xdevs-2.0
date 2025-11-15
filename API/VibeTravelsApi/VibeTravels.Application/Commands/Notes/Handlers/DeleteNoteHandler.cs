using VibeTravels.Application.Exceptions.Notes;
using VibeTravels.Application.Specifications.Notes;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Commands.Notes.Handlers;

public sealed class DeleteNoteHandler(INoteRepository noteRepository) : ICommandHandler<DeleteNote>
{
    public async Task HandleAsync(DeleteNote command)
    {
        UserId userId = command.UserId;
        NoteId noteId = command.NoteId;

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

        userNoteExists.MarkAsDeleted(DateTime.UtcNow);
        await noteRepository.UpdateNote(userNoteExists);
    }
}