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
    public class UpdateSectionEndpoint : Endpoint<UpdateSectionRequest>
    {
        private readonly IMediator _mediator;

        public UpdateSectionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/sections/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateSectionRequest req, CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Section? updated = await _mediator.Send(
                new UpdateSectionCommand(id, req.Name, req.SortOrder),
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
