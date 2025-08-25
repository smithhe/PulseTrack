using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Shared.Requests.Sections;

namespace PulseTrack.Api.Endpoints.Sections
{
    /// <summary>
    /// Endpoint for reordering sections within a project
    /// </summary>
    public class ReOrderSectionsEndpoint : Endpoint<ReorderSectionsRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the ReOrderSectionsEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public ReOrderSectionsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route with project ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects/{projectId:guid}/reorder-sections");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to reorder sections within a project
        /// </summary>
        /// <param name="req">The reorder sections request containing the new order of section IDs</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(ReorderSectionsRequest req, CancellationToken ct)
        {
            try
            {
                string? pidStr = HttpContext.Request.RouteValues["projectId"]?.ToString();
                if (!Guid.TryParse(pidStr, out Guid projectId))
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

                await _mediator.Send(
                    new ReorderSectionsCommand(projectId, req.OrderedSectionIds),
                    ct
                );

                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { success = true },
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to reorder sections", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
