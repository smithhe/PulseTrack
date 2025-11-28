using System;
using MediatR;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Commands.UpdateTeamMemberProfile;

public sealed record UpdateTeamMemberProfileCommand(
    Guid TeamMemberId,
    string DisplayName,
    string? Role,
    string? Email) : IRequest<ResponseBase>;

