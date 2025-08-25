using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.DueDates.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.DueDates;

namespace PulseTrack.Api.Endpoints.Items
{
    /// <summary>
    /// Endpoint for creating or updating due dates for items
    /// </summary>
    public class UpsertDueDateEndpoint : Endpoint<UpsertDueDateRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UpsertDueDateEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UpsertDueDateEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with PUT method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/items/{id:guid}/due-date");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to create or update a due date for an item
        /// </summary>
        /// <param name="req">The upsert due date request containing due date details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UpsertDueDateRequest req, CancellationToken ct)
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

                DueDate due = await _mediator.Send(
                    new UpsertDueDateCommand(
                        id,
                        req.DateUtc,
                        req.Timezone,
                        req.IsRecurring,
                        req.RecurrenceType,
                        req.RecurrenceInterval,
                        req.RecurrenceCount,
                        req.RecurrenceEndUtc,
                        req.RecurrenceWeeks
                    ),
                    ct
                );

                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    due,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to upsert due date", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
