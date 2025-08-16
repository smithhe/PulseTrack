using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Labels.Commands
{
    public record CreateLabelCommand(string Name, string? Color) : IRequest<Label>;
}
