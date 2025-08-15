using System.Collections.Generic;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Queries
{
    public record ListProjectsQuery() : IRequest<IReadOnlyList<Project>>;
}
