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
    public class UnPinItemEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public UnPinItemEndpoint(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public override void Configure()
        {
            this.Verbs(Http.POST);
            this.Routes("/items/{id:guid}/unpin");
            this.AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string? idStr = this.HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                this.HttpContext.Response.StatusCode = 404;
                return;
            }

            Item? updated = await this._mediator.Send(new UnPinItemCommand(id), ct);
            if (updated is null)
            {
                this.HttpContext.Response.StatusCode = 404;
                return;
            }

            this.HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                this.HttpContext.Response.Body,
                updated,
                cancellationToken: ct
            );
        }
    }
}
