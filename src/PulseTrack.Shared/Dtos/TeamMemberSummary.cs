using System;

namespace PulseTrack.Shared.Dtos;

public sealed record TeamMemberSummary(
    Guid Id,
    string DisplayName,
    string? Role,
    string? Email,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

