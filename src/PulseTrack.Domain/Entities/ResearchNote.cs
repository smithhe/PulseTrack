using PulseTrack.Domain.Abstractions;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Domain.Entities;

/// <summary>
/// Represents a note recorded against a research topic.
/// </summary>
public sealed class ResearchNote : AuditableEntity
{
    internal ResearchNote(
        Guid id,
        Guid topicId,
        ResearchNoteKind kind,
        string contentMarkdown,
        DateTime createdAtUtc,
        Guid? linkedWorkItemId)
        : base(id, createdAtUtc)
    {
        if (topicId == Guid.Empty)
        {
            throw new ArgumentException("Topic id cannot be empty.", nameof(topicId));
        }

        TopicId = topicId;
        Kind = kind;
        ContentMarkdown = NormalizeContent(contentMarkdown);
        LinkedWorkItemId = NormalizeLinkedWorkItem(linkedWorkItemId);
    }

    private ResearchNote() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        ContentMarkdown = string.Empty;
    }

    /// <summary>
    /// The identifier of the related research topic.
    /// </summary>
    public Guid TopicId { get; private set; }

    /// <summary>
    /// The classification of the note.
    /// </summary>
    public ResearchNoteKind Kind { get; private set; }

    /// <summary>
    /// The markdown content of the note.
    /// </summary>
    public string ContentMarkdown { get; private set; }

    /// <summary>
    /// The identifier of the linked work item, if any.
    /// </summary>
    public Guid? LinkedWorkItemId { get; private set; }

    /// <summary>
    /// Updates the note kind.
    /// </summary>
    /// <param name="kind">The new kind.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateKind(ResearchNoteKind kind, DateTime changedAtUtc)
    {
        Kind = kind;
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Updates the note content.
    /// </summary>
    /// <param name="contentMarkdown">The new markdown content.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateContent(string contentMarkdown, DateTime changedAtUtc)
    {
        ContentMarkdown = NormalizeContent(contentMarkdown);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Links the note to a work item.
    /// </summary>
    /// <param name="workItemId">The work item identifier.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void LinkWorkItem(Guid? workItemId, DateTime changedAtUtc)
    {
        LinkedWorkItemId = NormalizeLinkedWorkItem(workItemId);
        Touch(changedAtUtc);
    }

    private static string NormalizeContent(string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        content = content.Trim();

        if (content.Length > 4000)
        {
            throw new ArgumentOutOfRangeException(nameof(content), "Content must be 4000 characters or fewer.");
        }

        return content;
    }

    private static Guid? NormalizeLinkedWorkItem(Guid? workItemId)
    {
        if (workItemId == Guid.Empty)
        {
            return null;
        }

        return workItemId;
    }
}

