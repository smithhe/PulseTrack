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
    /// Endpoint for creating new labels
    /// </summary>
    public class CreateLabelEndpoint : Endpoint<CreateLabelRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the CreateLabelEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public CreateLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/labels");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to create a new label
        /// </summary>
        /// <param name="req">The create label request containing label details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CreateLabelRequest req, CancellationToken ct)
        {
            try
            {
                Label label = await _mediator.Send(new CreateLabelCommand(req.Name, req.Color), ct);
                await Send.ResponseAsync(label, (int)HttpStatusCode.Created, ct);
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
