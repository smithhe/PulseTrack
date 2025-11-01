namespace PulseTrack.Domain.Abstractions;

/// <summary>
/// Provides auditing support for entities by tracking creation and modification timestamps.
/// </summary>
public abstract class AuditableEntity : Entity
{
    protected AuditableEntity(Guid id, DateTime createdAtUtc)
        : base(id)
    {
        CreatedAtUtc = EnsureUtc(createdAtUtc);
        UpdatedAtUtc = CreatedAtUtc;
    }

    /// <summary>
    /// The UTC timestamp when the entity was created.
    /// </summary>
    public DateTime CreatedAtUtc { get; private set; }

    /// <summary>
    /// The UTC timestamp when the entity was last updated.
    /// </summary>
    public DateTime UpdatedAtUtc { get; private set; }

    /// <summary>
    /// Updates the modification timestamp.
    /// </summary>
    /// <param name="timestampUtc">The UTC timestamp of the change.</param>
    protected void Touch(DateTime timestampUtc)
    {
        UpdatedAtUtc = EnsureUtc(timestampUtc);
    }

    /// <summary>
    /// Normalizes a <see cref="DateTime"/> value to <see cref="DateTimeKind.Utc"/>.
    /// </summary>
    /// <param name="timestamp">The timestamp to normalize.</param>
    /// <returns>The timestamp represented in UTC.</returns>
    protected static DateTime EnsureUtc(DateTime timestamp)
    {
        return timestamp.Kind == DateTimeKind.Utc
            ? timestamp
            : DateTime.SpecifyKind(timestamp.ToUniversalTime(), DateTimeKind.Utc);
    }
}

