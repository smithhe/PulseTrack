using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Items.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Items
{
    public class GetItemHistoryEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public GetItemHistoryEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/items/{id:guid}/history");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Guid id = Route<Guid>("id");
            IReadOnlyList<ItemHistory> history = await _mediator.Send(
                new ListItemHistoryQuery(id),
                ct
            );

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                history,
                cancellationToken: ct
            );
        }
    }
}
