using System;

namespace PulseTrack.Shared.Dtos;

public sealed record UpdateTeamMemberProfileRequest(
    Guid TeamMemberId,
    string DisplayName,
    string? Role,
    string? Email);

