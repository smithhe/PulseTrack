using System;
using MediatR;

namespace PulseTrack.Application.Features.Projects.Commands
{
    public record DeleteProjectCommand(Guid Id) : IRequest<Unit>;
}


