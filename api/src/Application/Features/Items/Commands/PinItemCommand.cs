using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Commands
{
    public record PinItemCommand(Guid Id) : IRequest<Item?>;
}
