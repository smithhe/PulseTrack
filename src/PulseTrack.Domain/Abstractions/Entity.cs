namespace PulseTrack.Domain.Abstractions;

/// <summary>
/// Provides a base type for domain entities with identifier and concurrency token support.
/// </summary>
public abstract class Entity
{
    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }

    /// <summary>
    /// The unique identifier for the entity.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The concurrency token configured as a SQL Server rowversion.
    /// </summary>
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

