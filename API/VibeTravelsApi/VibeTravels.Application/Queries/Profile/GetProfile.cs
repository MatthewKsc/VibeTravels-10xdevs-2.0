using VibeTravels.Application.DTO;
using VibeTravels.Shared.CQRS;

namespace VibeTravels.Application.Queries.Profile;

public sealed class GetProfile : IQuery<ProfileDto?>
{
    public Guid UserId { get; set; }
}