using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Projects.Commands;

namespace PulseTrack.Api.Endpoints.Projects
{
    /// <summary>
    /// Endpoint for deleting projects
    /// </summary>
    public class DeleteProjectEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the DeleteProjectEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public DeleteProjectEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with DELETE method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/projects/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to delete a project by ID
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!Guid.TryParse(idStr, out Guid id))
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    HttpContext.Response.ContentType = "application/json";
                    await JsonSerializer.SerializeAsync(
                        HttpContext.Response.Body,
                        new { error = "Invalid project ID format" },
                        cancellationToken: ct
                    );
                    return;
                }

                await _mediator.Send(new DeleteProjectCommand(id), ct);
                HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to delete project", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
