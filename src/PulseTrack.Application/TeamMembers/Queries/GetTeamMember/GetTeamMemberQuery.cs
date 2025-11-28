using System;
using MediatR;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Queries.GetTeamMember;

public sealed record GetTeamMemberQuery(Guid TeamMemberId) : IRequest<Response<TeamMemberSummary>>;

