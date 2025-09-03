using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Sections.Commands;

namespace PulseTrack.Api.Endpoints.Sections
{
    /// <summary>
    /// Endpoint for deleting sections
    /// </summary>
    public class DeleteSectionEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the DeleteSectionEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public DeleteSectionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with DELETE method, route with section ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/sections/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to delete a section by ID
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!Guid.TryParse(idStr, out Guid id))
                {
                    await Send.ResponseAsync(new { error = "Invalid section ID format" }, (int)HttpStatusCode.BadRequest, ct);
                    return;
                }

                await _mediator.Send(new DeleteSectionCommand(id), ct);
                await Send.NoContentAsync(ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
