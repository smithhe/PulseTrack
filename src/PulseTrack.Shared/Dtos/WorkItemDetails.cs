using System;
using System.Collections.Generic;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Shared.Dtos;

public sealed record WorkItemDetails(
    Guid Id,
    string Title,
    string? DescriptionMarkdown,
    string? Notes,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid ProjectId,
    Guid? FeatureId,
    Guid? OwnerId,
    decimal? EstimatePoints,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc,
    DateTime? DueAtUtc,
    DateTime? CompletedAtUtc,
    IReadOnlyCollection<string> Tags);