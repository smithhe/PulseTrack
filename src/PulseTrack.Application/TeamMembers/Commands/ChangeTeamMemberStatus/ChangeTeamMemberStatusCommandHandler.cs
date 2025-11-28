using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Commands.ChangeTeamMemberStatus;

internal sealed class ChangeTeamMemberStatusCommandHandler : IRequestHandler<ChangeTeamMemberStatusCommand, ResponseBase>
{
    private readonly ITeamMemberRepository _repository;

    public ChangeTeamMemberStatusCommandHandler(ITeamMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseBase> Handle(ChangeTeamMemberStatusCommand request, CancellationToken cancellationToken)
    {
        TeamMember? teamMember = await _repository.GetAsync(request.TeamMemberId, cancellationToken);
        if (teamMember is null)
        {
            return ResponseBase.Failure("Team member not found.");
        }

        if (request.IsActive)
        {
            teamMember.Reactivate(request.ChangedAtUtc);
        }
        else
        {
            teamMember.Deactivate(request.ChangedAtUtc);
        }

        await _repository.UpdateAsync(teamMember, cancellationToken);

        return ResponseBase.Success("Team member status updated.");
    }
}

