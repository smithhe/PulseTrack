using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Items.Commands;

namespace PulseTrack.Api.Endpoints.Items
{
    /// <summary>
    /// Endpoint for deleting items
    /// </summary>
    public class DeleteItemEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the DeleteItemEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public DeleteItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with DELETE method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/items/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to delete an item by ID
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
                        new { error = "Invalid item ID format" },
                        cancellationToken: ct
                    );
                    return;
                }

                await _mediator.Send(new DeleteItemCommand(id), ct);
                HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to delete item", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
