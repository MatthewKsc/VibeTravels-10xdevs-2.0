using System.Linq.Expressions;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.User;
using VibeTravels.Shared.Specifications;

namespace VibeTravels.Application.Specifications.Notes;

internal sealed class NoteUserIdSpecification(UserId userId) : Specification<Note>
{
    public override Expression<Func<Note, bool>> ToExpression() => note => note.UserId == userId;
}

