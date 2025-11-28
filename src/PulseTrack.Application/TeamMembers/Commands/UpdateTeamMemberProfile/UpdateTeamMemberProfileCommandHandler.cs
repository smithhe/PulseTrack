using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Commands.UpdateTeamMemberProfile;

internal sealed class UpdateTeamMemberProfileCommandHandler : IRequestHandler<UpdateTeamMemberProfileCommand, ResponseBase>
{
    private readonly ITeamMemberRepository _repository;

    public UpdateTeamMemberProfileCommandHandler(ITeamMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseBase> Handle(UpdateTeamMemberProfileCommand request, CancellationToken cancellationToken)
    {
        TeamMember? teamMember = await _repository.GetAsync(request.TeamMemberId, cancellationToken);
        if (teamMember is null)
        {
            return ResponseBase.Failure("Team member not found.");
        }

        DateTime now = DateTime.UtcNow;

        teamMember.Rename(request.DisplayName, now);
        teamMember.ChangeRole(request.Role, now);
        teamMember.ChangeEmail(request.Email, now);

        await _repository.UpdateAsync(teamMember, cancellationToken);

        return ResponseBase.Success("Team member profile updated.");
    }
}

