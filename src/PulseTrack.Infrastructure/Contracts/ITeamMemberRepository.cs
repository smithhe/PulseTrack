using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Contracts;

public interface ITeamMemberRepository
{
    Task<TeamMember> CreateAsync(TeamMember teamMember, CancellationToken cancellationToken);

    Task<TeamMember?> GetAsync(Guid teamMemberId, CancellationToken cancellationToken);

    Task<IReadOnlyList<TeamMember>> ListAsync(bool? isActive, CancellationToken cancellationToken);

    Task<bool> ExistsAsync(Guid teamMemberId, CancellationToken cancellationToken);

    Task UpdateAsync(TeamMember teamMember, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid teamMemberId, CancellationToken cancellationToken);
}

