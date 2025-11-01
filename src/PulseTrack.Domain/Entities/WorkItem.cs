using System;
using System.Collections.Generic;
using PulseTrack.Domain.Abstractions;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Domain.Entities;

/// <summary>
/// Represents a unit of work tracked within a project.
/// </summary>
public sealed class WorkItem : AuditableEntity
{
    private readonly HashSet<string> _tags = new(StringComparer.OrdinalIgnoreCase);

    public WorkItem(
        Guid id,
        Guid projectId,
        string title,
        WorkItemStatus status,
        WorkItemPriority priority,
        DateTime createdAtUtc)
        : base(id, createdAtUtc)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project id cannot be empty.", nameof(projectId));
        }

        ProjectId = projectId;
        Title = NormalizeTitle(title);
        Status = status;
        Priority = priority;
    }

    private WorkItem() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        Title = string.Empty;
    }

    /// <summary>
    /// The identifier of the project that owns the work item.
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// The identifier of the associated feature, if any.
    /// </summary>
    public Guid? FeatureId { get; private set; }

    /// <summary>
    /// The identifier of the assigned owner, if any.
    /// </summary>
    public Guid? OwnerId { get; private set; }

    /// <summary>
    /// The title of the work item.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// The markdown description of the work item.
    /// </summary>
    public string? DescriptionMarkdown { get; private set; }

    /// <summary>
    /// The current status of the work item.
    /// </summary>
    public WorkItemStatus Status { get; private set; }

    /// <summary>
    /// The priority of the work item.
    /// </summary>
    public WorkItemPriority Priority { get; private set; }

    /// <summary>
    /// The story point estimate for the work item.
    /// </summary>
    public decimal? EstimatePoints { get; private set; }

    /// <summary>
    /// The due date for the work item in UTC, if any.
    /// </summary>
    public DateTime? DueAtUtc { get; private set; }

    /// <summary>
    /// The completion timestamp in UTC, if the work item is done.
    /// </summary>
    public DateTime? CompletedAtUtc { get; private set; }

    /// <summary>
    /// The set of tags applied to the work item.
    /// </summary>
    public IReadOnlyCollection<string> Tags => _tags;

    /// <summary>
    /// Updates the title of the work item.
    /// </summary>
    /// <param name="title">The new title.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateTitle(string title, DateTime changedAtUtc)
    {
        Title = NormalizeTitle(title);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Updates the description of the work item.
    /// </summary>
    /// <param name="markdown">The markdown text.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateDescription(string? markdown, DateTime changedAtUtc)
    {
        DescriptionMarkdown = string.IsNullOrWhiteSpace(markdown) ? null : markdown.Trim();
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Assigns a feature to the work item.
    /// </summary>
    /// <param name="featureId">The feature identifier.</param>
    /// <param name="assignedAtUtc">The assignment timestamp in UTC.</param>
    public void AssignFeature(Guid? featureId, DateTime assignedAtUtc)
    {
        if (featureId == Guid.Empty)
        {
            featureId = null;
        }

        FeatureId = featureId;
        Touch(assignedAtUtc);
    }

    /// <summary>
    /// Assigns an owner to the work item.
    /// </summary>
    /// <param name="ownerId">The owner identifier.</param>
    /// <param name="assignedAtUtc">The assignment timestamp in UTC.</param>
    public void AssignOwner(Guid? ownerId, DateTime assignedAtUtc)
    {
        if (ownerId == Guid.Empty)
        {
            ownerId = null;
        }

        OwnerId = ownerId;
        Touch(assignedAtUtc);
    }

    /// <summary>
    /// Changes the status of the work item.
    /// </summary>
    /// <param name="status">The new status.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void ChangeStatus(WorkItemStatus status, DateTime changedAtUtc)
    {
        Status = status;

        if (status == WorkItemStatus.Done)
        {
            CompletedAtUtc = EnsureUtc(changedAtUtc);
        }
        else if (CompletedAtUtc.HasValue)
        {
            CompletedAtUtc = null;
        }

        Touch(changedAtUtc);
    }

    /// <summary>
    /// Changes the priority of the work item.
    /// </summary>
    /// <param name="priority">The new priority.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void ChangePriority(WorkItemPriority priority, DateTime changedAtUtc)
    {
        Priority = priority;
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Sets the story point estimate.
    /// </summary>
    /// <param name="points">The estimate value.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void SetEstimate(decimal? points, DateTime changedAtUtc)
    {
        if (points.HasValue && points.Value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(points), points, "Estimate must be non-negative.");
        }

        EstimatePoints = points;
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Sets the due date for the work item.
    /// </summary>
    /// <param name="dueAtUtc">The due date in UTC.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void SetDueDate(DateTime? dueAtUtc, DateTime changedAtUtc)
    {
        DueAtUtc = dueAtUtc.HasValue ? EnsureUtc(dueAtUtc.Value) : null;
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Replaces the set of tags on the work item.
    /// </summary>
    /// <param name="tags">The tags to apply.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void ReplaceTags(IEnumerable<string> tags, DateTime changedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(tags);

        _tags.Clear();
        foreach (var tag in tags)
        {
            AddTagInternal(tag);
        }

        Touch(changedAtUtc);
    }

    /// <summary>
    /// Adds a tag to the work item.
    /// </summary>
    /// <param name="tag">The tag text.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    /// <returns><c>true</c> if the tag was added; otherwise, <c>false</c>.</returns>
    public bool AddTag(string tag, DateTime changedAtUtc)
    {
        var added = AddTagInternal(tag);
        if (added)
        {
            Touch(changedAtUtc);
        }

        return added;
    }

    /// <summary>
    /// Removes a tag from the work item.
    /// </summary>
    /// <param name="tag">The tag to remove.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    /// <returns><c>true</c> if the tag was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveTag(string tag, DateTime changedAtUtc)
    {
        var removed = _tags.Remove(tag);
        if (removed)
        {
            Touch(changedAtUtc);
        }

        return removed;
    }

    private bool AddTagInternal(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        tag = tag.Trim();

        if (tag.Length > 32)
        {
            throw new ArgumentOutOfRangeException(nameof(tag), tag, "Tags must be 32 characters or fewer.");
        }

        return _tags.Add(tag);
    }

    private static string NormalizeTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        title = title.Trim();

        if (title.Length > 256)
        {
            throw new ArgumentOutOfRangeException(nameof(title), title, "Title must be 256 characters or fewer.");
        }

        return title;
    }
}

