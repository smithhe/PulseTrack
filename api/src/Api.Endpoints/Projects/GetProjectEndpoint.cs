using System;
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
    /// Endpoint for retrieving a specific project by ID
    /// </summary>
    public class GetProjectEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the GetProjectEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public GetProjectEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/projects/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to retrieve a specific project by ID
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!Guid.TryParse(idStr, out Guid id))
                {
                    await Send.ResponseAsync(new { error = "Invalid project ID format" }, (int)HttpStatusCode.BadRequest, ct);
                    return;
                }

                Project? project = await _mediator.Send(new GetProjectByIdQuery(id), ct);
                if (project is null)
                {
                    await Send.ResponseAsync(new { error = "Project not found" }, (int)HttpStatusCode.NotFound, ct);
                    return;
                }

                await Send.OkAsync(project, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
