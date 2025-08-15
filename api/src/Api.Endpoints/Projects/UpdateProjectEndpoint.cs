using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Projects.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Projects;

namespace PulseTrack.Api.Endpoints.Projects
{
    public class UpdateProjectEndpoint : Endpoint<UpdateProjectRequest>
    {
        private readonly IMediator _mediator;

        public UpdateProjectEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/projects/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateProjectRequest req, CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Project? updated = await _mediator.Send(
                new UpdateProjectCommand(id, req.Name, req.Color, req.Icon, req.IsInbox),
                ct
            );
            if (updated is null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                updated,
                cancellationToken: ct
            );
        }
    }
}
