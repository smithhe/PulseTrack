using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Views.Commands;

namespace PulseTrack.Api.Endpoints.Views
{
    public class DeleteViewEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public DeleteViewEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/views/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Guid id = Route<Guid>("id");

            await _mediator.Send(new DeleteViewCommand(id), ct);
            HttpContext.Response.StatusCode = 204;
        }
    }
}
