using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Sections.Commands
{
    public record UpdateSectionCommand(Guid Id, string Name, int SortOrder) : IRequest<Section?>;
}


