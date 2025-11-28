using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Infrastructure.Data;

namespace PulseTrack.Infrastructure.Repositories;

internal sealed class TeamMemberRepository : ITeamMemberRepository
{
    private readonly PulseTrackDbContext _dbContext;

    public TeamMemberRepository(PulseTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TeamMember teamMember, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _dbContext.TeamMembers.AddAsync(teamMember, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public Task<TeamMember?> GetAsync(Guid teamMemberId, CancellationToken cancellationToken)
        => _dbContext.TeamMembers.FirstOrDefaultAsync(member => member.Id == teamMemberId, cancellationToken);

    public async Task<IReadOnlyList<TeamMember>> ListAsync(bool? isActive, CancellationToken cancellationToken)
    {
        IQueryable<TeamMember> query = _dbContext.TeamMembers.AsNoTracking();

        if (isActive.HasValue)
        {
            query = query.Where(member => member.IsActive == isActive.Value);
        }

        return await query
            .OrderBy(member => member.DisplayName)
            .ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(Guid teamMemberId, CancellationToken cancellationToken)
        => _dbContext.TeamMembers.AnyAsync(member => member.Id == teamMemberId, cancellationToken);

    public async Task UpdateAsync(TeamMember teamMember, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction =
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _dbContext.TeamMembers.Update(teamMember);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

