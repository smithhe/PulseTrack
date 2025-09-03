using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Views.Commands;
using PulseTrack.Shared.Requests.Views;

namespace PulseTrack.Api.Endpoints.Views
{
    /// <summary>
    /// Endpoint for updating existing views
    /// </summary>
    public class UpdateViewEndpoint : Endpoint<UpdateViewRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UpdateViewEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UpdateViewEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with PUT method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/views/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to update an existing view
        /// </summary>
        /// <param name="req">The update view request containing updated view details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UpdateViewRequest req, CancellationToken ct)
        {
            try
            {
                Guid id = Route<Guid>("id");
                await _mediator.Send(new UpdateViewCommand(id, req), ct);
                await Send.NoContentAsync(ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
