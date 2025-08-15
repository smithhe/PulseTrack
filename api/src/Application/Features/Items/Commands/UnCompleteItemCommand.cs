using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Commands
{
    public record UnCompleteItemCommand(Guid Id) : IRequest<Item?>;
}
