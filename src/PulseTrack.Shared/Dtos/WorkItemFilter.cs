using System;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Shared.Dtos;

public sealed record WorkItemFilter(
    Guid? ProjectId,
    Guid? FeatureId,
    Guid? OwnerId,
    WorkItemStatus? Status,
    WorkItemPriority? Priority,
    string? Tag);