using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Commands
{
    public record MoveItemCommand(Guid Id, Guid ProjectId, Guid? SectionId) : IRequest<Item?>;
}
