using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Dtos;

namespace PulseTrack.Application.TeamMembers.Shared;

internal static class TeamMemberMappingExtensions
{
    public static TeamMemberSummary ToSummary(this TeamMember entity)
    {
        return new TeamMemberSummary(
            entity.Id,
            entity.DisplayName,
            entity.Role,
            entity.Email,
            entity.IsActive,
            entity.CreatedAtUtc,
            entity.UpdatedAtUtc);
    }
}

