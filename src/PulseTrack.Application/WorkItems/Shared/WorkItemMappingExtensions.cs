using System.Collections.Generic;
using System.Linq;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Dtos;

namespace PulseTrack.Application.WorkItems.Shared;

internal static class WorkItemMappingExtensions
{
    public static WorkItemDetails ToDetails(this WorkItem entity)
    {
        IReadOnlyCollection<string> tags = entity.Tags.ToArray();

        return new WorkItemDetails(
            entity.Id,
            entity.Title,
            entity.DescriptionMarkdown,
            entity.Notes,
            entity.Status,
            entity.Priority,
            entity.ProjectId,
            entity.FeatureId,
            entity.OwnerId,
            entity.EstimatePoints,
            entity.CreatedAtUtc,
            entity.UpdatedAtUtc,
            entity.DueAtUtc,
            entity.CompletedAtUtc,
            tags);
    }

    public static WorkItemSummary ToSummary(this WorkItem entity)
    {
        IReadOnlyCollection<string> tags = entity.Tags.ToArray();

        return new WorkItemSummary(
            entity.Id,
            entity.Title,
            entity.Status,
            entity.Priority,
            entity.ProjectId,
            entity.FeatureId,
            entity.OwnerId,
            entity.CreatedAtUtc,
            entity.DueAtUtc,
            tags);
    }
}

