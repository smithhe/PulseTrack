using System;
using System.Collections.Generic;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Shared.Dtos;

public sealed record CreateWorkItemRequest(
    Guid ProjectId,
    string Title,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid? FeatureId,
    Guid? OwnerId,
    string? DescriptionMarkdown,
    string? Notes,
    decimal? EstimatePoints,
    DateTime? DueAtUtc,
    IReadOnlyCollection<string>? Tags);