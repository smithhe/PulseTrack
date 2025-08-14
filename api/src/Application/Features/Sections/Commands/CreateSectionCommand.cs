using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Sections.Commands
{
    public record CreateSectionCommand(Guid ProjectId, string Name, int SortOrder) : IRequest<Section>;
}


