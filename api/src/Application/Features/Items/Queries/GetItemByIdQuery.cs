using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Queries
{
    public record GetItemByIdQuery(Guid Id) : IRequest<Item?>;
}
