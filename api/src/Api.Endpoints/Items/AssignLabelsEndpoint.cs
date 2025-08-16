using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Shared.Requests.Items;

namespace PulseTrack.Api.Endpoints.Items
{
    public class AssignLabelEndpoint : Endpoint<AssignLabelRequest>
    {
        private readonly IMediator _mediator;

        public AssignLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items/{id:guid}/labels");
            AllowAnonymous();
        }

        public override async Task HandleAsync(AssignLabelRequest req, CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            await _mediator.Send(new AssignLabelCommand(id, req.LabelId), ct);

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                new { ok = true },
                cancellationToken: ct
            );
        }
    }
}
