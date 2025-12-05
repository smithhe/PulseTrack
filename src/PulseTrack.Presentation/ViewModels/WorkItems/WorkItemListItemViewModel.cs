using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using PulseTrack.Domain.Enums;
using PulseTrack.Presentation.WorkItems.Models;

namespace PulseTrack.Presentation.ViewModels.WorkItems;

/// <summary>
/// View-model projected from a work item summary, used in lists and detail panes.
/// </summary>
public sealed partial class WorkItemListItemViewModel : ObservableObject
{
    public WorkItemListItemViewModel(WorkItemListItem model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public WorkItemListItem Model { get; }

    public Guid Id => Model.Id;

    public string Key => Model.Key;

    public string Title => Model.Title;

    public string ProjectName => Model.ProjectName;

    public string? FeatureName => Model.FeatureName;

    public string? OwnerDisplayName => Model.OwnerDisplayName;

    public WorkItemStatus Status => Model.Status;

    public WorkItemPriority Priority => Model.Priority;

    public DateTime CreatedAtUtc => Model.CreatedAtUtc;

    public DateTime? DueAtUtc => Model.DueAtUtc;

    public DateTime? CompletedAtUtc => Model.CompletedAtUtc;

    public string? Summary => Model.Summary;

    public IReadOnlyList<string> Tags => Model.Tags;

    public string StatusLabel => Status.ToString();

    public string PriorityLabel => Priority.ToString();

    public bool IsOverdue =>
        DueAtUtc.HasValue &&
        DueAtUtc.Value < DateTime.UtcNow &&
        Status != WorkItemStatus.Done;
}

