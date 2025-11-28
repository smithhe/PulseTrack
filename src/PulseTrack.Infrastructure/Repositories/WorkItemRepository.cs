using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PulseTrack.Domain.Entities;
using PulseTrack.Domain.Enums;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Infrastructure.Data;

namespace PulseTrack.Infrastructure.Repositories;

internal sealed class WorkItemRepository : IWorkItemRepository
{
    private readonly PulseTrackDbContext _dbContext;

    public WorkItemRepository(PulseTrackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WorkItem> CreateAsync(WorkItem workItem, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _dbContext.WorkItems.AddAsync(workItem, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return workItem;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public Task<WorkItem?> GetAsync(Guid workItemId, CancellationToken cancellationToken)
        => _dbContext.WorkItems.FirstOrDefaultAsync(item => item.Id == workItemId, cancellationToken);

    public async Task<IReadOnlyList<WorkItem>> ListAsync(WorkItemRepositoryFilter filter, CancellationToken cancellationToken)
    {
        IQueryable<WorkItem> query = _dbContext.WorkItems.AsNoTracking();

        if (filter.ProjectId.HasValue)
        {
            query = query.Where(item => item.ProjectId == filter.ProjectId);
        }

        if (filter.FeatureId.HasValue)
        {
            query = query.Where(item => item.FeatureId == filter.FeatureId);
        }

        if (filter.OwnerId.HasValue)
        {
            query = query.Where(item => item.OwnerId == filter.OwnerId);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(item => item.Status == filter.Status);
        }

        if (filter.Priority.HasValue)
        {
            query = query.Where(item => item.Priority == filter.Priority);
        }

        List<WorkItem> results = await query
            .OrderByDescending(item => item.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(filter.Tag))
        {
            results = results
                .Where(item => item.Tags.Contains(filter.Tag))
                .ToList();
        }

        return results;
    }

    public async Task UpdateAsync(WorkItem workItem, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _dbContext.WorkItems.Update(workItem);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task ChangeStatusAsync(Guid workItemId, WorkItemStatus status, DateTime changedAtUtc, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            WorkItem? entity = await _dbContext.WorkItems.FirstOrDefaultAsync(item => item.Id == workItemId, cancellationToken);
            if (entity is null)
            {
                throw new InvalidOperationException($"Work item {workItemId} was not found.");
            }

            entity.ChangeStatus(status, changedAtUtc);
            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task DeleteAsync(Guid workItemId, CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            WorkItem? entity = await _dbContext.WorkItems.FirstOrDefaultAsync(item => item.Id == workItemId, cancellationToken);
            if (entity is null)
            {
                return;
            }

            _dbContext.WorkItems.Remove(entity);
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

