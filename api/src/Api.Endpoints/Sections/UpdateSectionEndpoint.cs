using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Sections;

namespace PulseTrack.Api.Endpoints.Sections
{
    /// <summary>
    /// Endpoint for updating existing sections
    /// </summary>
    public class UpdateSectionEndpoint : Endpoint<UpdateSectionRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UpdateSectionEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UpdateSectionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with PUT method, route with section ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/sections/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to update an existing section
        /// </summary>
        /// <param name="req">The update section request containing updated section details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UpdateSectionRequest req, CancellationToken ct)
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
                        new { error = "Invalid section ID format" },
                        cancellationToken: ct
                    );
                    return;
                }

                Section? updated = await _mediator.Send(
                    new UpdateSectionCommand(id, req.Name, req.SortOrder),
                    ct
                );
                if (updated is null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    HttpContext.Response.ContentType = "application/json";
                    await JsonSerializer.SerializeAsync(
                        HttpContext.Response.Body,
                        new { error = "Section not found" },
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
                    new { error = "Failed to update section", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
