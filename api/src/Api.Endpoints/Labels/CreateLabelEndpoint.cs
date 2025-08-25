using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
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

                HttpContext.Response.StatusCode = StatusCodes.Status201Created;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    label,
                    cancellationToken: ct
                );
            }
            catch (Exception ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                HttpContext.Response.ContentType = "application/json";
                await JsonSerializer.SerializeAsync(
                    HttpContext.Response.Body,
                    new { error = "Failed to create label", details = ex.Message },
                    cancellationToken: ct
                );
            }
        }
    }
}
