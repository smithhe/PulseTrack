using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Items.Queries;

namespace PulseTrack.Api.Endpoints.Items
{
    public class ListItemsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public ListItemsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/items");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Guid? projectId = null;
            if (Query<Guid?>("projectId") is Guid pid)
                projectId = pid;

            var items = await _mediator.Send(new ListItemsQuery(projectId), ct);
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(HttpContext.Response.Body, items, cancellationToken: ct);
        }
    }
}


