using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Queries
{
    public record GetProjectByIdQuery(Guid Id) : IRequest<Project?>;
}
