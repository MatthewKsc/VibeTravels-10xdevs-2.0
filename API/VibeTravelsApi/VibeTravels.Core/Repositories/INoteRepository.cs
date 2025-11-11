using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Core.Repositories;

public interface INoteRepository
{
    Task<Note?> GetNoteOrDefault(Specification<Note> specification);
    Task<IReadOnlyCollection<Note>> GetNotes(Specification<Note> specification);
    Task AddNote(Note note);
    Task UpdateNote(Note note);
    Task DeleteNote(Note note);
}