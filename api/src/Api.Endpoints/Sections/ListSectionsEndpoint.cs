using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Sections.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Sections
{
    public class ListSectionsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public ListSectionsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/projects/{projectId:guid}/sections");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string? pidStr = HttpContext.Request.RouteValues["projectId"]?.ToString();
            if (!Guid.TryParse(pidStr, out Guid projectId))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            IReadOnlyList<Section> sections = await _mediator.Send(
                new ListSectionsByProjectQuery(projectId),
                ct
            );
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                sections,
                cancellationToken: ct
            );
        }
    }
}
