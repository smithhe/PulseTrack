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
    public class CompleteItemEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public CompleteItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items/{id:guid}/complete");
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
            Item? updated = await _mediator.Send(new CompleteItemCommand(id), ct);
            if (updated is null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(HttpContext.Response.Body, updated, cancellationToken: ct);
        }
    }

    public class UncompleteItemEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;
        public UncompleteItemEndpoint(IMediator mediator) { _mediator = mediator; }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items/{id:guid}/uncomplete");
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
            Item? updated = await _mediator.Send(new UncompleteItemCommand(id), ct);
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


