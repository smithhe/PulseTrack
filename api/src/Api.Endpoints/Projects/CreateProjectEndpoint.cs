using FastEndpoints;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PulseTrack.Application.Features.Projects.Commands;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Api.Endpoints.Projects
{
    public record CreateProjectRequest(string Name, string? Color, string? Icon, bool IsInbox);

    public class CreateProjectEndpoint : Endpoint<CreateProjectRequest>
    {
        private readonly IMediator _mediator;

        public CreateProjectEndpoint(IMediator mediator) { _mediator = mediator; }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateProjectRequest req, CancellationToken ct)
        {
            Project project = await _mediator.Send(new CreateProjectCommand(req.Name, req.Color, req.Icon, req.IsInbox), ct);

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(HttpContext.Response.Body, project, cancellationToken: ct);
        }
    }
}


