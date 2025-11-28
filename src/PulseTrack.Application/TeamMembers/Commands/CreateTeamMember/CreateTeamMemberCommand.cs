using MediatR;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Commands.CreateTeamMember;

public sealed record CreateTeamMemberCommand(
    string DisplayName,
    string? Role,
    string? Email) : IRequest<Response<TeamMemberSummary>>;

