using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Projects.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Projects
{
    /// <summary>
    /// Endpoint for retrieving all projects
    /// </summary>
    public class ListProjectsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the ListProjectsEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public ListProjectsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/projects");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to list all projects
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                IReadOnlyList<Project> projects = await _mediator.Send(new ListProjectsQuery(), ct);
                await Send.OkAsync(projects, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
