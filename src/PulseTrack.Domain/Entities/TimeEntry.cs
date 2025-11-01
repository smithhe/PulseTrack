using PulseTrack.Domain.Abstractions;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Domain.Entities;

/// <summary>
/// Represents a recorded span of time associated with a work item.
/// </summary>
public sealed class TimeEntry : AuditableEntity
{
    public TimeEntry(
        Guid id,
        Guid workItemId,
        DateTime startUtc,
        DateTime endUtc,
        TimeEntrySource source,
        string? notes,
        DateTime createdAtUtc)
        : base(id, createdAtUtc)
    {
        if (workItemId == Guid.Empty)
        {
            throw new ArgumentException("Work item id cannot be empty.", nameof(workItemId));
        }

        WorkItemId = workItemId;
        Source = source;
        Notes = NormalizeNotes(notes);

        SetPeriod(startUtc, endUtc);
    }

    private TimeEntry() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
    }

    /// <summary>
    /// The identifier of the work item associated with the entry.
    /// </summary>
    public Guid WorkItemId { get; private set; }

    /// <summary>
    /// The UTC start time of the entry.
    /// </summary>
    public DateTime StartUtc { get; private set; }

    /// <summary>
    /// The UTC end time of the entry.
    /// </summary>
    public DateTime EndUtc { get; private set; }

    /// <summary>
    /// The origin of the time entry.
    /// </summary>
    public TimeEntrySource Source { get; private set; }

    /// <summary>
    /// Supplemental notes captured with the entry.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// The duration of the entry.
    /// </summary>
    public TimeSpan Duration => EndUtc - StartUtc;

    /// <summary>
    /// Adjusts the start and end times of the entry.
    /// </summary>
    /// <param name="startUtc">The new UTC start time.</param>
    /// <param name="endUtc">The new UTC end time.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void AdjustPeriod(DateTime startUtc, DateTime endUtc, DateTime changedAtUtc)
    {
        SetPeriod(startUtc, endUtc);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Changes the source of the entry.
    /// </summary>
    /// <param name="source">The new source value.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void ChangeSource(TimeEntrySource source, DateTime changedAtUtc)
    {
        Source = source;
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Updates the notes on the entry.
    /// </summary>
    /// <param name="notes">The note text.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void UpdateNotes(string? notes, DateTime changedAtUtc)
    {
        Notes = NormalizeNotes(notes);
        Touch(changedAtUtc);
    }

    private void SetPeriod(DateTime startUtc, DateTime endUtc)
    {
        startUtc = EnsureUtc(startUtc);
        endUtc = EnsureUtc(endUtc);

        if (endUtc < startUtc)
        {
            throw new ArgumentOutOfRangeException(nameof(endUtc), endUtc, "End time must be after start time.");
        }

        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    private static string? NormalizeNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            return null;
        }

        notes = notes.Trim();

        if (notes.Length > 1024)
        {
            throw new ArgumentOutOfRangeException(nameof(notes), "Notes must be 1024 characters or fewer.");
        }

        return notes;
    }
}

