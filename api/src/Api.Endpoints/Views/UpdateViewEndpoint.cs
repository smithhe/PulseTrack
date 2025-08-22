using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Views.Commands;
using PulseTrack.Shared.Requests.Views;

namespace PulseTrack.Api.Endpoints.Views
{
    public class UpdateViewEndpoint : Endpoint<UpdateViewRequest>
    {
        private readonly IMediator _mediator;

        public UpdateViewEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/views/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateViewRequest req, CancellationToken ct)
        {
            Guid id = Route<Guid>("id");

            await _mediator.Send(new UpdateViewCommand(id, req), ct);
            HttpContext.Response.StatusCode = 204;
        }
    }
}
