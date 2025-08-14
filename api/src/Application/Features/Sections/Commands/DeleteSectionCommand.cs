using System;
using MediatR;

namespace PulseTrack.Application.Features.Sections.Commands
{
    public record DeleteSectionCommand(Guid Id) : IRequest<Unit>;
}


