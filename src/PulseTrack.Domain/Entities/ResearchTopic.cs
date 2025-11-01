using PulseTrack.Domain.Abstractions;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Domain.Entities;

/// <summary>
/// Represents an area of investigation with supporting notes and findings.
/// </summary>
public sealed class ResearchTopic : AuditableEntity
{
    private readonly List<ResearchNote> _notes = new();

    public ResearchTopic(Guid id, string title, string problemArea, string goal, DateTime createdAtUtc)
        : base(id, createdAtUtc)
    {
        Title = NormalizeText(title, nameof(title), 200);
        ProblemArea = NormalizeText(problemArea, nameof(problemArea), 200);
        Goal = NormalizeText(goal, nameof(goal), 400);
    }

    private ResearchTopic() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        Title = string.Empty;
        ProblemArea = string.Empty;
        Goal = string.Empty;
    }

    /// <summary>
    /// The title of the research topic.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// The problem area being studied.
    /// </summary>
    public string ProblemArea { get; private set; }

    /// <summary>
    /// The goal of the research topic.
    /// </summary>
    public string Goal { get; private set; }

    /// <summary>
    /// The notes recorded for the topic.
    /// </summary>
    public IReadOnlyCollection<ResearchNote> Notes => _notes.AsReadOnly();

    /// <summary>
    /// Adds a note to the topic.
    /// </summary>
    /// <param name="noteId">The identifier of the note.</param>
    /// <param name="kind">The type of the note.</param>
    /// <param name="contentMarkdown">The markdown content of the note.</param>
    /// <param name="createdAtUtc">The creation timestamp in UTC.</param>
    /// <param name="linkedWorkItemId">An optional linked work item identifier.</param>
    /// <returns>The newly created note.</returns>
    public ResearchNote AddNote(
        Guid noteId,
        ResearchNoteKind kind,
        string contentMarkdown,
        DateTime createdAtUtc,
        Guid? linkedWorkItemId = null)
    {
        if (noteId == Guid.Empty)
        {
            throw new ArgumentException("Note id cannot be empty.", nameof(noteId));
        }

        if (linkedWorkItemId == Guid.Empty)
        {
            linkedWorkItemId = null;
        }

        var note = new ResearchNote(noteId, Id, kind, contentMarkdown, createdAtUtc, linkedWorkItemId);
        _notes.Add(note);
        Touch(createdAtUtc);
        return note;
    }

    /// <summary>
    /// Removes a note from the topic.
    /// </summary>
    /// <param name="noteId">The identifier of the note to remove.</param>
    /// <param name="removedAtUtc">The removal timestamp in UTC.</param>
    /// <returns><c>true</c> if the note was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveNote(Guid noteId, DateTime removedAtUtc)
    {
        var removed = _notes.RemoveAll(n => n.Id == noteId) > 0;
        if (removed)
        {
            Touch(removedAtUtc);
        }

        return removed;
    }

    /// <summary>
    /// Updates the title of the topic.
    /// </summary>
    /// <param name="title">The new title.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateTitle(string title, DateTime changedAtUtc)
    {
        Title = NormalizeText(title, nameof(title), 200);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Updates the problem area description.
    /// </summary>
    /// <param name="problemArea">The new problem area.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateProblemArea(string problemArea, DateTime changedAtUtc)
    {
        ProblemArea = NormalizeText(problemArea, nameof(problemArea), 200);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Updates the goal of the topic.
    /// </summary>
    /// <param name="goal">The new goal text.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateGoal(string goal, DateTime changedAtUtc)
    {
        Goal = NormalizeText(goal, nameof(goal), 400);
        Touch(changedAtUtc);
    }

    private static string NormalizeText(string value, string parameterName, int maxLength)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
        value = value.Trim();

        if (value.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(parameterName, value, $"Value must be {maxLength} characters or fewer.");
        }

        return value;
    }
}

