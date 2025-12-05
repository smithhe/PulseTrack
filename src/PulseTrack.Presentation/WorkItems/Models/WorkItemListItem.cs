using System;
using System.Collections.Generic;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Presentation.WorkItems.Models;

/// <summary>
/// Lightweight DTO used by the presentation layer to render work item summaries.
/// </summary>
public sealed record WorkItemListItem(
    Guid Id,
    string Key,
    string Title,
    string ProjectName,
    string? FeatureName,
    string? OwnerDisplayName,
    WorkItemStatus Status,
    WorkItemPriority Priority,
    DateTime CreatedAtUtc,
    DateTime? DueAtUtc,
    DateTime? CompletedAtUtc,
    string? Summary,
    IReadOnlyList<string> Tags);

