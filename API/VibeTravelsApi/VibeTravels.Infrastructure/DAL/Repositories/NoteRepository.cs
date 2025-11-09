using Microsoft.EntityFrameworkCore;
using VibeTravels.Core.Entities;
using VibeTravels.Core.Repositories;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Infrastructure.DAL.Repositories;

internal sealed class NoteRepository(VibeTravelsContext context) : INoteRepository
{
    public Task<Note?> GetNoteOrDefault(Specification<Note> specification) =>
        context.Notes
            .Where(specification.ToExpression())
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyCollection<Note>> GetNotes(Specification<Note> specification) =>
        await context.Notes
            .Where(specification.ToExpression())
            .ToArrayAsync();

    public async Task AddNote(Note note)
    {
        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
    }

    public async Task UpdateNote(Note note)
    {
        context.Notes.Update(note);
        await context.SaveChangesAsync();
    }

    public async Task DeleteNote(Note note)
    {
        context.Notes.Remove(note);
        await context.SaveChangesAsync();
    }
}

