using System;
using MediatR;

namespace PulseTrack.Application.Features.Items.Commands
{
    public record DeleteItemCommand(Guid Id) : IRequest<Unit>;
}
