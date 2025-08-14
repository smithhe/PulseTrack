using System;
using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Queries
{
    public record ListItemsQuery(Guid? ProjectId) : IRequest<IReadOnlyList<Item>>;
}


