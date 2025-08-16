using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Labels;

namespace PulseTrack.Api.Endpoints.Labels
{
    public class UpdateLabelEndpoint : Endpoint<UpdateLabelRequest>
    {
        private readonly IMediator _mediator;

        public UpdateLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/labels/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateLabelRequest req, CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Label? label = await _mediator.Send(
                new UpdateLabelCommand(id, req.Name, req.Color),
                ct
            );
            if (label is null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                label,
                cancellationToken: ct
            );
        }
    }
}
