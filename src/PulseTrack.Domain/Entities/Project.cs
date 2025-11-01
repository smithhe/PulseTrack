using PulseTrack.Domain.Abstractions;

namespace PulseTrack.Domain.Entities;

/// <summary>
/// Represents a project that groups features and work items.
/// </summary>
public sealed class Project : AuditableEntity
{
    private readonly List<Feature> _features = new();

    public Project(Guid id, string name, string key, string? colorHex, DateTime createdAtUtc)
        : base(id, createdAtUtc)
    {
        Name = NormalizeName(name);
        Key = NormalizeKey(key);
        Color = NormalizeColor(colorHex);
    }

    private Project() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        Name = string.Empty;
        Key = string.Empty;
    }

    /// <summary>
    /// The display name of the project.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Short identifier (e.g., ABC) that can prefix work item numbers.
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// Hex color in #RRGGBB or #AARRGGBB format; null uses default theme.
    /// </summary>
    public string? Color { get; private set; }

    /// <summary>
    /// The collection of features associated with the project.
    /// </summary>
    public IReadOnlyCollection<Feature> Features => _features.AsReadOnly();

    /// <summary>
    /// Adds a feature to the project.
    /// </summary>
    /// <param name="featureId">The identifier of the new feature.</param>
    /// <param name="name">The name of the feature.</param>
    /// <param name="createdAtUtc">The creation timestamp in UTC.</param>
    /// <returns>The newly created feature.</returns>
    public Feature AddFeature(Guid featureId, string name, DateTime createdAtUtc)
    {
        if (featureId == Guid.Empty)
        {
            throw new ArgumentException("Feature id cannot be empty.", nameof(featureId));
        }

        var feature = new Feature(featureId, Id, name, createdAtUtc);
        _features.Add(feature);
        Touch(createdAtUtc);
        return feature;
    }

    /// <summary>
    /// Removes a feature from the project.
    /// </summary>
    /// <param name="featureId">The identifier of the feature to remove.</param>
    /// <param name="removedAtUtc">The removal timestamp in UTC.</param>
    /// <returns><c>true</c> if the feature was removed; otherwise, <c>false</c>.</returns>
    public bool RemoveFeature(Guid featureId, DateTime removedAtUtc)
    {
        var removed = _features.RemoveAll(f => f.Id == featureId) > 0;
        if (removed)
        {
            Touch(removedAtUtc);
        }

        return removed;
    }

    /// <summary>
    /// Updates the project name.
    /// </summary>
    /// <param name="name">The new project name.</param>
    /// <param name="renamedAtUtc">The rename timestamp in UTC.</param>
    public void Rename(string name, DateTime renamedAtUtc)
    {
        Name = NormalizeName(name);
        Touch(renamedAtUtc);
    }

    /// <summary>
    /// Updates the project key.
    /// </summary>
    /// <param name="key">The new project key.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void ChangeKey(string key, DateTime changedAtUtc)
    {
        Key = NormalizeKey(key);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Sets the color used to represent the project in the UI.
    /// </summary>
    /// <param name="colorHex">The color hex value.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void SetColor(string? colorHex, DateTime changedAtUtc)
    {
        Color = NormalizeColor(colorHex);
        Touch(changedAtUtc);
    }

    private static string NormalizeName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return name.Trim();
    }

    private static string NormalizeKey(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        key = key.Trim().ToUpperInvariant();

        if (key.Length is < 2 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(key), key, "Project key must be between 2 and 8 characters.");
        }

        return key;
    }

    private static string? NormalizeColor(string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return null;
        }

        color = color.Trim();

        if (color.StartsWith('#') == false)
        {
            throw new ArgumentException("Color must be a hex value starting with '#'.", nameof(color));
        }

        if (color.Length is not 7 and not 9)
        {
            throw new ArgumentException("Color must be in #RRGGBB or #AARRGGBB format.", nameof(color));
        }

        return color.ToUpperInvariant();
    }
}

