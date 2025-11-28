using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.TeamMembers.Shared;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Queries.GetTeamMember;

internal sealed class GetTeamMemberQueryHandler : IRequestHandler<GetTeamMemberQuery, Response<TeamMemberSummary>>
{
    private readonly ITeamMemberRepository _repository;

    public GetTeamMemberQueryHandler(ITeamMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<TeamMemberSummary>> Handle(GetTeamMemberQuery request, CancellationToken cancellationToken)
    {
        TeamMember? teamMember = await _repository.GetAsync(request.TeamMemberId, cancellationToken);
        if (teamMember is null)
        {
            return Response<TeamMemberSummary>.Failure("Team member not found.");
        }

        return Response<TeamMemberSummary>.Success(teamMember.ToSummary());
    }
}

