using System;
using MediatR;

namespace PulseTrack.Application.Features.Views.Commands
{
    public record DeleteViewCommand(Guid Id) : IRequest;
}
