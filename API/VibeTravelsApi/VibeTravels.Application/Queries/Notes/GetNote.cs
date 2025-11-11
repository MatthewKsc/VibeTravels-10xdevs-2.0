﻿using VibeTravels.Application.DTO;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Queries.Notes;

public sealed class GetNote : IQuery<NoteDto>
{
    public Guid UserId { get; set; }
    public Guid NoteId { get; set; }
}