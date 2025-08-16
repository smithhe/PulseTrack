using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Labels.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Labels
{
    public class ListLabelsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public ListLabelsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/labels");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IReadOnlyList<Label> labels = await _mediator.Send(new ListLabelsQuery(), ct);

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                labels,
                cancellationToken: ct
            );
        }
    }
}
