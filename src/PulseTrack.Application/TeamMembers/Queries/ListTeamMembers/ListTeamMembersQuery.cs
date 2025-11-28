using System.Collections.Generic;
using MediatR;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Queries.ListTeamMembers;

public sealed record ListTeamMembersQuery(bool? IsActive) : IRequest<Response<IReadOnlyList<TeamMemberSummary>>>;

