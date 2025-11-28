using System;

namespace PulseTrack.Shared.Dtos;

public sealed record ChangeTeamMemberStatusRequest(
    Guid TeamMemberId,
    bool IsActive,
    DateTime ChangedAtUtc);

