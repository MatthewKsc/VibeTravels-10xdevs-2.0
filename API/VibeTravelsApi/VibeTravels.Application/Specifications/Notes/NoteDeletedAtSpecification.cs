using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Notes;

internal sealed class NoteDeletedAtSpecification(bool includeDeletedNotes) : Specification<Note>
{
    public override Expression<Func<Note, bool>> ToExpression()
    {
        if (includeDeletedNotes)
        {
            return note => true;
        }
        
        return note => note.DeletedAt == null;
    }
}