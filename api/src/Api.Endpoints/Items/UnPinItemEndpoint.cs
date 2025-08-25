using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Items
{
    /// <summary>
    /// Endpoint for unpinning an item
    /// </summary>
    public class UnPinItemEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UnPinItemEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UnPinItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items/{id:guid}/unpin");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to unpin an item
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

                Item? updated = await _mediator.Send(new UnPinItemCommand(id), ct);
                if (updated is null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    HttpContext.Response.ContentType = "application/json";
                    await JsonSerializer.SerializeAsync(
                        HttpContext.Response.Body,
                        new { error = "Item not found" },
                        cancellationToken: ct
                    );
                    return;
                }

                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    updated,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to unpin item", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
