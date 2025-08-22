using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Views.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Views
{
    public class GetViewByIdEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public GetViewByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/views/{id:guid}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            Guid id = Route<Guid>("id");
            View? view = await _mediator.Send(new GetViewByIdQuery(id), ct);

            if (view is null)
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                view,
                cancellationToken: ct
            );
        }
    }
}
