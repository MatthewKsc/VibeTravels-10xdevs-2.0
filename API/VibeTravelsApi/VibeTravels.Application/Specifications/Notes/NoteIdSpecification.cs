using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Notes;

internal sealed class NoteIdSpecification(NoteId noteId) : Specification<Note>
{
    public override Expression<Func<Note, bool>> ToExpression() => note => note.Id == noteId;
}