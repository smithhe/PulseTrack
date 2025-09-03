using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Labels;

namespace PulseTrack.Api.Endpoints.Labels
{
    /// <summary>
    /// Endpoint for updating existing labels
    /// </summary>
    public class UpdateLabelEndpoint : Endpoint<UpdateLabelRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UpdateLabelEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UpdateLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with PUT method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/labels/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to update an existing label
        /// </summary>
        /// <param name="req">The update label request containing updated label details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UpdateLabelRequest req, CancellationToken ct)
        {
            try
            {
                string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!Guid.TryParse(idStr, out Guid id))
                {
                    await Send.ResponseAsync(
                        new { error = "Invalid label ID format" },
                        (int)HttpStatusCode.BadRequest,
                        ct
                    );
                    return;
                }

                Label? label = await _mediator.Send(
                    new UpdateLabelCommand(id, req.Name, req.Color),
                    ct
                );
                if (label is null)
                {
                    await Send.ResponseAsync(
                        new { error = "Label not found" },
                        (int)HttpStatusCode.NotFound,
                        ct
                    );
                    return;
                }

                await Send.OkAsync(label, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(
                    new { error = "Unexpected Error Occurred" },
                    (int)HttpStatusCode.InternalServerError,
                    ct
                );
            }
        }
    }
}
