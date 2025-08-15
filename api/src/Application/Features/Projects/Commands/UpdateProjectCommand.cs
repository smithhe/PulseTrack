using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Commands
{
    public record UpdateProjectCommand(
        Guid Id,
        string Name,
        string? Color,
        string? Icon,
        bool IsInbox
    ) : IRequest<Project?>;
}
