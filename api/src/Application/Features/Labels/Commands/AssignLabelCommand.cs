using System;
using MediatR;

namespace PulseTrack.Application.Features.Labels.Commands
{
    public record AssignLabelCommand(Guid ItemId, Guid LabelId) : IRequest<Unit>;
}
