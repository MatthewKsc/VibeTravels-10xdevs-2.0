using VibeTravels.Application.DTO;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Queries.Notes;

public sealed class GetNotes : IQuery<NoteDto[]>
{
    public Guid UserId { get; set; }
    //TODO: For MVP only, add filtering, pagination, etc. later
}