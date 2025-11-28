namespace PulseTrack.Shared.Dtos;

public sealed record CreateTeamMemberRequest(
    string DisplayName,
    string? Role,
    string? Email);

