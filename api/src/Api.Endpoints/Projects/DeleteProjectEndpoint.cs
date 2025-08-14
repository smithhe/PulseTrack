using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Projects.Commands;

namespace PulseTrack.Api.Endpoints.Projects
{
    public class DeleteProjectEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;
        public DeleteProjectEndpoint(IMediator mediator) { _mediator = mediator; }

        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/projects/{id:guid}");
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

            await _mediator.Send(new DeleteProjectCommand(id), ct);
            HttpContext.Response.StatusCode = 204;
        }
    }
}


