using System;
using System.Collections.Generic;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Shared.Dtos;

public sealed record UpdateWorkItemRequest(
    Guid WorkItemId,
    string? Title,
    string? DescriptionMarkdown,
    Guid? FeatureId,
    Guid? OwnerId,
    WorkItemPriority? Priority,
    decimal? EstimatePoints,
    DateTime? DueAtUtc,
    IReadOnlyCollection<string>? Tags);