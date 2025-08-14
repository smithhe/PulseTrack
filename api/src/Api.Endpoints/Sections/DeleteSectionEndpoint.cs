using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Sections.Commands;

namespace PulseTrack.Api.Endpoints.Sections
{
    public class DeleteSectionEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;
        public DeleteSectionEndpoint(IMediator mediator) { _mediator = mediator; }

        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/sections/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            await _mediator.Send(new DeleteSectionCommand(id), ct);
            HttpContext.Response.StatusCode = 204;
        }
    }
}


