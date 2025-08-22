using MediatR;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Views;

namespace PulseTrack.Application.Features.Views.Commands
{
    public record CreateViewCommand(CreateViewRequest Request) : IRequest<View>;
}
