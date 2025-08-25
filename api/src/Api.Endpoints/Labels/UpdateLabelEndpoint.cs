using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Labels;

namespace PulseTrack.Api.Endpoints.Labels
{
    /// <summary>
    /// Endpoint for updating existing labels
    /// </summary>
    public class UpdateLabelEndpoint : Endpoint<UpdateLabelRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UpdateLabelEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UpdateLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with PUT method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/labels/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to update an existing label
        /// </summary>
        /// <param name="req">The update label request containing updated label details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UpdateLabelRequest req, CancellationToken ct)
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
                        new { error = "Invalid label ID format" },
                        cancellationToken: ct
                    );
                    return;
                }

                Label? label = await _mediator.Send(
                    new UpdateLabelCommand(id, req.Name, req.Color),
                    ct
                );
                if (label is null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    HttpContext.Response.ContentType = "application/json";
                    await JsonSerializer.SerializeAsync(
                        HttpContext.Response.Body,
                        new { error = "Label not found" },
                        cancellationToken: ct
                    );
                    return;
                }

                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    label,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to update label", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
