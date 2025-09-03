using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Views.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Views
{
    /// <summary>
    /// Endpoint for retrieving a specific view by ID
    /// </summary>
    public class GetViewByIdEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the GetViewByIdEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public GetViewByIdEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/views/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to retrieve a specific view by ID
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                Guid id = Route<Guid>("id");
                View? view = await _mediator.Send(new GetViewByIdQuery(id), ct);
                if (view is null)
                {
                    await Send.ResponseAsync(new { error = "View not found" }, (int)HttpStatusCode.NotFound, ct);
                    return;
                }
                await Send.OkAsync(view, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
