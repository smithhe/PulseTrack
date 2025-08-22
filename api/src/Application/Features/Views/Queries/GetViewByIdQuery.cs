using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Views.Queries
{
    public record GetViewByIdQuery(Guid Id) : IRequest<View?>;
}
