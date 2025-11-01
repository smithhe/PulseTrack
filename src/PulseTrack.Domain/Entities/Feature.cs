using PulseTrack.Domain.Abstractions;

namespace PulseTrack.Domain.Entities;

/// <summary>
/// Represents a feature that belongs to a project and groups related work items.
/// </summary>
public sealed class Feature : AuditableEntity
{
    internal Feature(Guid id, Guid projectId, string name, DateTime createdAtUtc)
        : base(id, createdAtUtc)
    {
        if (projectId == Guid.Empty)
        {
            throw new ArgumentException("Project id cannot be empty.", nameof(projectId));
        }

        ProjectId = projectId;
        Name = NormalizeName(name);
    }

    private Feature() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        Name = string.Empty;
    }

    /// <summary>
    /// The identifier of the project that owns the feature.
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// The name of the feature.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Indicates whether the feature is archived.
    /// </summary>
    public bool IsArchived { get; private set; }

    /// <summary>
    /// Renames the feature.
    /// </summary>
    /// <param name="name">The new name.</param>
    /// <param name="renamedAtUtc">The rename timestamp in UTC.</param>
    public void Rename(string name, DateTime renamedAtUtc)
    {
        Name = NormalizeName(name);
        Touch(renamedAtUtc);
    }

    /// <summary>
    /// Archives the feature.
    /// </summary>
    /// <param name="archivedAtUtc">The archive timestamp in UTC.</param>
    public void Archive(DateTime archivedAtUtc)
    {
        if (IsArchived)
        {
            return;
        }

        IsArchived = true;
        Touch(archivedAtUtc);
    }

    /// <summary>
    /// Restores the feature from an archived state.
    /// </summary>
    /// <param name="restoredAtUtc">The restore timestamp in UTC.</param>
    public void Restore(DateTime restoredAtUtc)
    {
        if (!IsArchived)
        {
            return;
        }

        IsArchived = false;
        Touch(restoredAtUtc);
    }

    private static string NormalizeName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return name.Trim();
    }
}

