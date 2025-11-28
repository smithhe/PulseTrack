using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.TeamMembers.Shared;
using PulseTrack.Domain.Entities;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.TeamMembers.Commands.CreateTeamMember;

internal sealed class CreateTeamMemberCommandHandler : IRequestHandler<CreateTeamMemberCommand, Response<TeamMemberSummary>>
{
    private readonly ITeamMemberRepository _repository;

    public CreateTeamMemberCommandHandler(ITeamMemberRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<TeamMemberSummary>> Handle(CreateTeamMemberCommand request, CancellationToken cancellationToken)
    {
        DateTime now = DateTime.UtcNow;

        TeamMember teamMember = new TeamMember(
            id: Guid.NewGuid(),
            displayName: request.DisplayName,
            role: request.Role,
            email: request.Email,
            joinedAtUtc: now);

        TeamMember created = await _repository.CreateAsync(teamMember, cancellationToken);

        return Response<TeamMemberSummary>.Success(created.ToSummary(), "Team member created.");
    }
}

