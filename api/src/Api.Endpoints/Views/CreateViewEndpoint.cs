using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Views.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Views;

namespace PulseTrack.Api.Endpoints.Views
{
    /// <summary>
    /// Endpoint for creating new views
    /// </summary>
    public class CreateViewEndpoint : Endpoint<CreateViewRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the CreateViewEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public CreateViewEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/views");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to create a new view
        /// </summary>
        /// <param name="req">The create view request containing view details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CreateViewRequest req, CancellationToken ct)
        {
            try
            {
                View view = await _mediator.Send(new CreateViewCommand(req), ct);
                await Send.ResponseAsync(view, (int)HttpStatusCode.Created, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
