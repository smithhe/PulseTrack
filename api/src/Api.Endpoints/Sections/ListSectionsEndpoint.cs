using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Sections.Queries;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Sections
{
    /// <summary>
    /// Endpoint for retrieving all sections for a specific project
    /// </summary>
    public class ListSectionsEndpoint : EndpointWithoutRequest
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the ListSectionsEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public ListSectionsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with GET method, route with project ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/projects/{projectId:guid}/sections");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to list all sections for a specific project
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                string? pidStr = HttpContext.Request.RouteValues["projectId"]?.ToString();
                if (!Guid.TryParse(pidStr, out Guid projectId))
                {
                    await Send.ResponseAsync(
                        new { error = "Invalid project ID format" },
                        (int)HttpStatusCode.BadRequest,
                        ct
                    );
                    return;
                }

                IReadOnlyList<Section> sections = await _mediator.Send(
                    new ListSectionsByProjectQuery(projectId),
                    ct
                );
                await Send.OkAsync(sections, ct);
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
