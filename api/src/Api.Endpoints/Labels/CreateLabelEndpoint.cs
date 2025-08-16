using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Labels;

namespace PulseTrack.Api.Endpoints.Labels
{
    public class CreateLabelEndpoint : Endpoint<CreateLabelRequest>
    {
        private readonly IMediator _mediator;

        public CreateLabelEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/labels");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateLabelRequest req, CancellationToken ct)
        {
            Label label = await _mediator.Send(new CreateLabelCommand(req.Name, req.Color), ct);
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                label,
                cancellationToken: ct
            );
        }
    }
}
