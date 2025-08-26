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
    /// Endpoint for creating new items
    /// </summary>
    public class CreateItemEndpoint : Endpoint<CreateItemRequest>
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Creates a new instance of the CreateItemEndpoint
        /// </summary>
        /// <param name="mediator">MediatR mediator for handling requests</param>
        public CreateItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Configures the endpoint with POST method, route, and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the request to create a new item
        /// </summary>
        /// <param name="req">The create item request containing item details</param>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CreateItemRequest req, CancellationToken ct)
        {
            try
            {
                Item item = await _mediator.Send(
                    new CreateItemCommand(
                        req.ProjectId,
                        req.SectionId,
                        req.Content,
                        req.DescriptionMd,
                        req.Priority,
                        req.Pinned
                    ),
                    ct
                );

                await Send.ResponseAsync(item, (int)HttpStatusCode.Created, ct);
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
