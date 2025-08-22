using System;
using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Views.Queries
{
    public record ListViewsQuery(Guid? ProjectId) : IRequest<IReadOnlyList<View>>;
}
