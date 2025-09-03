using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Labels.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Labels
{
    /// <summary>
    /// Endpoint for retrieving all labels
    /// </summary>
    public class ListLabelsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the ListLabelsEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public ListLabelsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/labels");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to list all labels
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                IReadOnlyList<Label> labels = await _mediator.Send(new ListLabelsQuery(), ct);
                await Send.OkAsync(labels, ct);
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
