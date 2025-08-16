using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Labels.Commands;

namespace PulseTrack.Api.Endpoints.Labels
{
    public class DeleteLabelEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public DeleteLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/labels/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            await _mediator.Send(new DeleteLabelCommand(id), ct);
            HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
        }
    }
}
