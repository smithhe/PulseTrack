using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Sections;

namespace PulseTrack.Api.Endpoints.Sections
{
    public class CreateSectionEndpoint : Endpoint<CreateSectionRequest>
    {
        private readonly IMediator _mediator;
        public CreateSectionEndpoint(IMediator mediator) { _mediator = mediator; }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects/{projectId:guid}/sections");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateSectionRequest req, CancellationToken ct)
        {
            string? pidStr = HttpContext.Request.RouteValues["projectId"]?.ToString();
            if (!Guid.TryParse(pidStr, out Guid projectId))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Section section = await _mediator.Send(new CreateSectionCommand(projectId, req.Name, req.SortOrder), ct);
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(HttpContext.Response.Body, section, cancellationToken: ct);
        }
    }
}


