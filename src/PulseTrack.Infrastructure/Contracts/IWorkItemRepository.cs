using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Entities;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Infrastructure.Contracts;

public sealed record WorkItemRepositoryFilter(
    Guid? ProjectId,
    Guid? FeatureId,
    Guid? OwnerId,
    WorkItemStatus? Status,
    WorkItemPriority? Priority,
    string? Tag);

public interface IWorkItemRepository
{
    Task<WorkItem> CreateAsync(WorkItem workItem, CancellationToken cancellationToken);

    Task<WorkItem?> GetAsync(Guid workItemId, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorkItem>> ListAsync(WorkItemRepositoryFilter filter, CancellationToken cancellationToken);

    Task UpdateAsync(WorkItem workItem, CancellationToken cancellationToken);

    Task ChangeStatusAsync(Guid workItemId, WorkItemStatus status, DateTime changedAtUtc, CancellationToken cancellationToken);

    Task DeleteAsync(Guid workItemId, CancellationToken cancellationToken);
}

