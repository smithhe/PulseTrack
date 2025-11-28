using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.TeamMembers.Shared;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Queries.ListTeamMembers;

internal sealed class ListTeamMembersQueryHandler : IRequestHandler<ListTeamMembersQuery, Response<IReadOnlyList<TeamMemberSummary>>>
{
    private readonly ITeamMemberRepository _repository;

    public ListTeamMembersQueryHandler(ITeamMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<IReadOnlyList<TeamMemberSummary>>> Handle(ListTeamMembersQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<TeamMemberSummary> summaries = (await _repository.ListAsync(request.IsActive, cancellationToken))
            .Select(member => member.ToSummary())
            .ToArray();

        return Response<IReadOnlyList<TeamMemberSummary>>.Success(summaries);
    }
}

