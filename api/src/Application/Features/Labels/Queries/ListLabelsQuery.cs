using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Labels.Queries
{
    public record ListLabelsQuery() : IRequest<IReadOnlyList<Label>>;
}
