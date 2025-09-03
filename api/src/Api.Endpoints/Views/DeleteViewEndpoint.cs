using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Views.Commands;

namespace PulseTrack.Api.Endpoints.Views
{
    /// <summary>
    /// Endpoint for deleting views
    /// </summary>
    public class DeleteViewEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the DeleteViewEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public DeleteViewEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with DELETE method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.DELETE);
            Routes("/views/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to delete a view by ID
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                Guid id = Route<Guid>("id");
                await _mediator.Send(new DeleteViewCommand(id), ct);
                await Send.NoContentAsync(ct);
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
