using System;
using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Sections.Queries
{
    public record ListSectionsByProjectQuery(Guid ProjectId) : IRequest<IReadOnlyList<Section>>;
}


