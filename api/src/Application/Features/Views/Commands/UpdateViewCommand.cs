using System;
using MediatR;
using PulseTrack.Shared.Requests.Views;

namespace PulseTrack.Application.Features.Views.Commands
{
    public record UpdateViewCommand(Guid Id, UpdateViewRequest Request) : IRequest;
}
