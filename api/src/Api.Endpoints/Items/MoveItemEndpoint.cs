using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Items;

namespace PulseTrack.Api.Endpoints.Items
{
    /// <summary>
    /// Endpoint for moving an item to a different project or section
    /// </summary>
    public class MoveItemEndpoint : Endpoint<MoveItemRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the MoveItemEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public MoveItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items/{id:guid}/move");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to move an item to a different project or section
        /// </summary>
        /// <param name="req">The move item request containing target project and section IDs</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(MoveItemRequest req, CancellationToken ct)
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

                Item? updated = await _mediator.Send(
                    new MoveItemCommand(id, req.ProjectId, req.SectionId),
                    ct
                );
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
                    new { error = "Failed to move item", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
