using System;
using System.Net;
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
                    await Send.ResponseAsync(new { error = "Invalid section ID format" }, (int)HttpStatusCode.BadRequest, ct);
                    return;
                }

                Section? updated = await _mediator.Send(
                    new UpdateSectionCommand(id, req.Name, req.SortOrder),
                    ct
                );
                if (updated is null)
                {
                    await Send.ResponseAsync(new { error = "Section not found" }, (int)HttpStatusCode.NotFound, ct);
                    return;
                }

                await Send.OkAsync(updated, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
