using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Labels.Queries
{
    public record GetLabelByIdQuery(Guid Id) : IRequest<Label?>;
}
