using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Labels.Commands
{
    public record UpdateLabelCommand(Guid Id, string Name, string? Color) : IRequest<Label?>;
}
