using System;
using System.Collections.Generic;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Shared.Dtos;

public sealed record WorkItemSummary(
    Guid Id,
    string Title,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    Guid ProjectId,
    Guid? FeatureId,
    Guid? OwnerId,
    DateTime CreatedAtUtc,
    DateTime? DueAtUtc,
    IReadOnlyCollection<string> Tags);