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
    public record UpdateItemRequest(Guid ProjectId, Guid? SectionId, string Content, string? DescriptionMd, int Priority, bool Pinned);

    public class UpdateItemEndpoint : Endpoint<UpdateItemRequest>
    {
        private readonly IMediator _mediator;
        public UpdateItemEndpoint(IMediator mediator) { _mediator = mediator; }

        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/items/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateItemRequest req, CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            Item? updated = await _mediator.Send(new UpdateItemCommand(id, req.ProjectId, req.SectionId, req.Content, req.DescriptionMd, req.Priority, req.Pinned), ct);
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


