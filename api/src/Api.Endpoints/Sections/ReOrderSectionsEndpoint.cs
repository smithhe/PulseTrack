using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Shared.Requests.Sections;

namespace PulseTrack.Api.Endpoints.Sections
{
    /// <summary>
    /// Endpoint for reordering sections within a project
    /// </summary>
    public class ReOrderSectionsEndpoint : Endpoint<ReorderSectionsRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the ReOrderSectionsEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public ReOrderSectionsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route with project ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects/{projectId:guid}/reorder-sections");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to reorder sections within a project
        /// </summary>
        /// <param name="req">The reorder sections request containing the new order of section IDs</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(ReorderSectionsRequest req, CancellationToken ct)
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

                await _mediator.Send(
                    new ReorderSectionsCommand(projectId, req.OrderedSectionIds),
                    ct
                );
                await Send.OkAsync(new { success = true }, ct);
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
