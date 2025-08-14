using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Items
{
    public record MoveItemRequest(Guid ProjectId, Guid? SectionId);

    public class MoveItemEndpoint : Endpoint<MoveItemRequest>
    {
        private readonly IMediator _mediator;
        public MoveItemEndpoint(IMediator mediator) { _mediator = mediator; }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items/{id:guid}/move");
            AllowAnonymous();
        }

        public override async Task HandleAsync(MoveItemRequest req, CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }
            Item? updated = await _mediator.Send(new MoveItemCommand(id, req.ProjectId, req.SectionId), ct);
            if (updated is null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(HttpContext.Response.Body, updated, cancellationToken: ct);
        }
    }
}


