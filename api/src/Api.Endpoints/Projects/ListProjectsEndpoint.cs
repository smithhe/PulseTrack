using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Projects.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Projects
{
    public class ListProjectsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        public ListProjectsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/projects");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IReadOnlyList<Project> projects = await _mediator.Send(new ListProjectsQuery(), ct);

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                projects,
                cancellationToken: ct
            );
        }
    }
}
