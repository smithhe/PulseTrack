using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Views.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Views;

namespace PulseTrack.Api.Endpoints.Views
{
    public class CreateViewEndpoint : Endpoint<CreateViewRequest>
    {
        private readonly IMediator _mediator;

        public CreateViewEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/views");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateViewRequest req, CancellationToken ct)
        {
            View view = await _mediator.Send(new CreateViewCommand(req), ct);

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                view,
                cancellationToken: ct
            );
        }
    }
}
