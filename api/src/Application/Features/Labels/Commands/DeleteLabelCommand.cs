using System;
using MediatR;

namespace PulseTrack.Application.Features.Labels.Commands
{
    public record DeleteLabelCommand(Guid Id) : IRequest<Unit>;
}
