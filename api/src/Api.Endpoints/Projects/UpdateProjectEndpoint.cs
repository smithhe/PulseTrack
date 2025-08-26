using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Projects.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Projects;

namespace PulseTrack.Api.Endpoints.Projects
{
    /// <summary>
    /// Endpoint for updating existing projects
    /// </summary>
    public class UpdateProjectEndpoint : Endpoint<UpdateProjectRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UpdateProjectEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UpdateProjectEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with PUT method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/projects/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to update an existing project
        /// </summary>
        /// <param name="req">The update project request containing updated project details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UpdateProjectRequest req, CancellationToken ct)
        {
            try
            {
                string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!Guid.TryParse(idStr, out Guid id))
                {
                    await Send.ResponseAsync(new { error = "Invalid project ID format" }, (int)HttpStatusCode.BadRequest, ct);
                    return;
                }

                Project? updated = await _mediator.Send(
                    new UpdateProjectCommand(id, req.Name, req.Color, req.Icon, req.IsInbox),
                    ct
                );
                if (updated is null)
                {
                    await Send.ResponseAsync(new { error = "Project not found" }, (int)HttpStatusCode.NotFound, ct);
                    return;
                }

                await Send.OkAsync(updated, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
