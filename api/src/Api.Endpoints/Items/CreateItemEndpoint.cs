using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Items.Commands;

namespace PulseTrack.Api.Endpoints.Items
{
    public record CreateItemRequest(Guid ProjectId, Guid? SectionId, string Content, string? DescriptionMd, int Priority, bool Pinned);

    public class CreateItemEndpoint : Endpoint<CreateItemRequest>
    {
        private readonly IMediator _mediator;

        public CreateItemEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/items");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateItemRequest req, CancellationToken ct)
        {
            var item = await _mediator.Send(new CreateItemCommand(req.ProjectId, req.SectionId, req.Content, req.DescriptionMd, req.Priority, req.Pinned), ct);
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(HttpContext.Response.Body, item, cancellationToken: ct);
        }
    }
}


