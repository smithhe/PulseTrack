using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Shared.Requests.Items;

namespace PulseTrack.Api.Endpoints.Items
{
    /// <summary>
    /// Endpoint for unassigning a label from an item
    /// </summary>
    public class UnassignLabelEndpoint : Endpoint<UnassignLabelRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UnassignLabelEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UnassignLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with DELETE method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/items/{id:guid}/labels");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to unassign a label from an item
        /// </summary>
        /// <param name="req">The unassign label request containing the label ID to remove</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UnassignLabelRequest req, CancellationToken ct)
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
                        new { error = "Invalid item ID format" },
                        cancellationToken: ct
                    );
                    return;
                }

                await _mediator.Send(new UnassignLabelCommand(id, req.LabelId), ct);
                HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to unassign label from item", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
