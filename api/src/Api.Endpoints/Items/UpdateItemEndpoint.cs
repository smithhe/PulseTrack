using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Items.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Items;

namespace PulseTrack.Api.Endpoints.Items
{
    /// <summary>
    /// Endpoint for updating existing items
    /// </summary>
    public class UpdateItemEndpoint : Endpoint<UpdateItemRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the UpdateItemEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public UpdateItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with PUT method, route with ID parameter, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/items/{id:guid}");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to update an existing item
        /// </summary>
        /// <param name="req">The update item request containing updated item details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(UpdateItemRequest req, CancellationToken ct)
        {
            try
            {
                string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
                if (!Guid.TryParse(idStr, out Guid id))
                {
                    await Send.ResponseAsync(
                        new { error = "Invalid item ID format" },
                        (int)HttpStatusCode.BadRequest,
                        ct
                    );
                    return;
                }

                Item? updated = await _mediator.Send(
                    new UpdateItemCommand(
                        id,
                        req.ProjectId,
                        req.SectionId,
                        req.Content,
                        req.DescriptionMd,
                        req.Priority,
                        req.Pinned
                    ),
                    ct
                );

                if (updated is null)
                {
                    await Send.ResponseAsync(
                        new { error = "Item not found" },
                        (int)HttpStatusCode.NotFound,
                        ct
                    );
                    return;
                }

                await Send.OkAsync(updated, ct);
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
