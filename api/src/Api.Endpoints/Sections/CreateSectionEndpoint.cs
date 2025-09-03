using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Sections;

namespace PulseTrack.Api.Endpoints.Sections
{
    /// <summary>
    /// Endpoint for creating new sections within a project
    /// </summary>
    public class CreateSectionEndpoint : Endpoint<CreateSectionRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the CreateSectionEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public CreateSectionEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route with project ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects/{projectId:guid}/sections");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to create a new section within a project
        /// </summary>
        /// <param name="req">The create section request containing section details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CreateSectionRequest req, CancellationToken ct)
        {
            try
            {
                string? pidStr = HttpContext.Request.RouteValues["projectId"]?.ToString();
                if (!Guid.TryParse(pidStr, out Guid projectId))
                {
                    await Send.ResponseAsync(new { error = "Invalid project ID format" }, (int)HttpStatusCode.BadRequest, ct);
                    return;
                }

                Section section = await _mediator.Send(
                    new CreateSectionCommand(projectId, req.Name, req.SortOrder),
                    ct
                );
                await Send.ResponseAsync(section, (int)HttpStatusCode.Created, ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(new { error = "Unexpected Error Occurred" }, (int)HttpStatusCode.InternalServerError, ct);
            }
        }
    }
}
