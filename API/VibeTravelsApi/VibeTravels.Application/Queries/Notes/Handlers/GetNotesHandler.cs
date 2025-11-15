using VibeTravels.Application.DTO;
using VibeTravels.Application.Specifications.Notes;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Queries.Notes.Handlers;

public sealed class GetNotesHandler(INoteRepository noteRepository) : IQueryHandler<GetNotes, NoteDto[]>
{
    public async Task<NoteDto[]> HandleAsync(GetNotes query)
    {
        UserId userId = query.UserId;

        Specification<Note> specification = new NoteUserIdSpecification(userId)
            .And(new NoteDeletedAtSpecification(false));
        
        IReadOnlyCollection<Note> notes = await noteRepository.GetNotes(specification);

        return notes
            .Select(note => new NoteDto(
                Id: note.Id,
                Title:note.Title,
                Location: note.Location,
                Content:note.Body,
                CreatedAt: note.CreatedAt,
                UpdatedAt: note.UpdatedAt))
            .ToArray();
    }
}

