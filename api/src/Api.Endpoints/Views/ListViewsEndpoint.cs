using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Views.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Views
{
    /// <summary>
    /// Endpoint for retrieving all views, optionally filtered by project
    /// </summary>
    public class ListViewsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the ListViewsEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public ListViewsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/views");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to list all views, optionally filtered by project ID
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                Guid? projectId = null;
                if (Query<Guid?>("projectId") is { } pid)
                {
                    projectId = pid;
                }

                IReadOnlyList<View> views = await _mediator.Send(new ListViewsQuery(projectId), ct);
                await Send.OkAsync(views, ct);
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
