using System;
using System.Collections.Generic;
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
    /// Endpoint for retrieving the history of changes for a specific item
    /// </summary>
    public class GetItemHistoryEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the GetItemHistoryEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public GetItemHistoryEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/items/{id:guid}/history");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to retrieve the history of changes for a specific item
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                Guid id = Route<Guid>("id");
                IReadOnlyList<ItemHistory> history = await _mediator.Send(
                    new ListItemHistoryQuery(id),
                    ct
                );

                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    history,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to retrieve item history", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
