using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Items.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Items
{
    /// <summary>
    /// Endpoint for retrieving a specific item by ID
    /// </summary>
    public class GetItemEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the GetItemEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public GetItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/items/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to retrieve a specific item by ID
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

                Item? item = await _mediator.Send(new GetItemByIdQuery(id), ct);
                if (item is null)
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
                    item,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to retrieve item", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
