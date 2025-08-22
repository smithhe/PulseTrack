using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Views.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Views
{
    public class ListViewsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public ListViewsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/views");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Guid? projectId = null;
            if (Query<Guid?>("projectId") is Guid pid)
            {
                projectId = pid;
            }

            IReadOnlyList<View> views = await _mediator.Send(new ListViewsQuery(projectId), ct);

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                views,
                cancellationToken: ct
            );
        }
    }
}
