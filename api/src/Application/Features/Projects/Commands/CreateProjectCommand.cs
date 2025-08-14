using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Projects.Commands
{
    public record CreateProjectCommand(string Name, string? Color, string? Icon, bool IsInbox) : IRequest<Project>;
}


