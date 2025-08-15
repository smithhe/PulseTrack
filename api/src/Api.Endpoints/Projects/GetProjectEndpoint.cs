using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Projects.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Projects
{
    public class GetProjectEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public GetProjectEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
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

            Project? project = await _mediator.Send(new GetProjectByIdQuery(id), ct);
            if (project is null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                project,
                cancellationToken: ct
            );
        }
    }
}
