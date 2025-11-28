using System;
using MediatR;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Commands.ChangeTeamMemberStatus;

public sealed record ChangeTeamMemberStatusCommand(
    Guid TeamMemberId,
    bool IsActive,
    DateTime ChangedAtUtc) : IRequest<ResponseBase>;

