using VibeTravels.Application.DTO;
using VibeTravels.Application.Exceptions.Notes;
using VibeTravels.Application.Queries.Notes;
using VibeTravels.Application.Specifications.Notes;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.CQRS;
using VibeTravels.Shared.Specifications;

public sealed class GetNoteHandler(INoteRepository noteRepository) : IQueryHandler<GetNote, NoteDto>
{
    public async Task<NoteDto> HandleAsync(GetNote query)
    {
        UserId userId = query.UserId;
        NoteId noteId = query.NoteId;

        Specification<Note> specification = new NoteIdSpecification(noteId)
            .And(new NoteUserIdSpecification(userId))
            .And(new NoteDeletedAtSpecification(false));
        
        Note? note = await noteRepository.GetNoteOrDefault(specification);

        if (note is null)
            throw new NoteNotFoundException(noteId);
        
        return new NoteDto(
            Id: note.Id,
            Title:note.Title,
            Location: note.Location,
            Content:note.Body,
            CreatedAt: note.CreatedAt,
            UpdatedAt: note.UpdatedAt);
    }
}