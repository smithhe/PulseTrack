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
    /// Endpoint for assigning a label to an item
    /// </summary>
    public class AssignLabelsEndpoint : Endpoint<AssignLabelRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the AssignLabelsEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public AssignLabelsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items/{id:guid}/labels");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to assign a label to an item
        /// </summary>
        /// <param name="req">The assign label request containing the label ID to assign</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(AssignLabelRequest req, CancellationToken ct)
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

                await _mediator.Send(new AssignLabelCommand(id, req.LabelId), ct);

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
                    new { error = "Failed to assign label to item", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
