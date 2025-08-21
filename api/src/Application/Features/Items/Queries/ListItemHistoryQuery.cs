using System;
using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Queries
{
    public record ListItemHistoryQuery(Guid ItemId) : IRequest<IReadOnlyList<ItemHistory>>;
}


