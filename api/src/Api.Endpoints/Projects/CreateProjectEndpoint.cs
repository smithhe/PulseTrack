using System;
using System.Text.Json;
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
    /// Endpoint for creating new projects
    /// </summary>
    public class CreateProjectEndpoint : Endpoint<CreateProjectRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the CreateProjectEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public CreateProjectEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to create a new project
        /// </summary>
        /// <param name="req">The create project request containing project details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CreateProjectRequest req, CancellationToken ct)
        {
            try
            {
                Project project = await _mediator.Send(
                    new CreateProjectCommand(req.Name, req.Color, req.Icon, req.IsInbox),
                    ct
                );

                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    project,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to create project", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
