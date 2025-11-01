using PulseTrack.Domain.Abstractions;

namespace PulseTrack.Domain.Entities;

/// <summary>
/// Represents a member of the team.
/// </summary>
public sealed class TeamMember : AuditableEntity
{
    public TeamMember(Guid id, string displayName, string? role, string? email, DateTime joinedAtUtc)
        : base(id, joinedAtUtc)
    {
        DisplayName = NormalizeName(displayName);
        Role = NormalizeRole(role);
        Email = NormalizeEmail(email);
        IsActive = true;
    }

    private TeamMember() : base(Guid.NewGuid(), DateTime.UtcNow)
    {
        DisplayName = string.Empty;
        IsActive = true;
    }

    /// <summary>
    /// The display name of the team member.
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// The role or title of the team member.
    /// </summary>
    public string? Role { get; private set; }

    /// <summary>
    /// The email address of the team member.
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    /// Indicates whether the team member is active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Updates the display name of the team member.
    /// </summary>
    /// <param name="displayName">The new display name.</param>
    /// <param name="renamedAtUtc">The rename timestamp in UTC.</param>
    public void Rename(string displayName, DateTime renamedAtUtc)
    {
        DisplayName = NormalizeName(displayName);
        Touch(renamedAtUtc);
    }

    /// <summary>
    /// Assigns a new role to the team member.
    /// </summary>
    /// <param name="role">The new role.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void ChangeRole(string? role, DateTime changedAtUtc)
    {
        Role = NormalizeRole(role);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Updates the email address of the team member.
    /// </summary>
    /// <param name="email">The new email.</param>
    /// <param name="changedAtUtc">The change timestamp in UTC.</param>
    public void ChangeEmail(string? email, DateTime changedAtUtc)
    {
        Email = NormalizeEmail(email);
        Touch(changedAtUtc);
    }

    /// <summary>
    /// Marks the team member as inactive.
    /// </summary>
    /// <param name="deactivatedAtUtc">The deactivate timestamp in UTC.</param>
    public void Deactivate(DateTime deactivatedAtUtc)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        Touch(deactivatedAtUtc);
    }

    /// <summary>
    /// Reactivates the team member.
    /// </summary>
    /// <param name="reactivatedAtUtc">The reactivation timestamp in UTC.</param>
    public void Reactivate(DateTime reactivatedAtUtc)
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        Touch(reactivatedAtUtc);
    }

    private static string NormalizeName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return name.Trim();
    }

    private static string? NormalizeRole(string? role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            return null;
        }

        return role.Trim();
    }

    private static string? NormalizeEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        email = email.Trim();

        if (!email.Contains('@'))
        {
            throw new ArgumentException("Email must contain '@'.", nameof(email));
        }

        return email;
    }
}

