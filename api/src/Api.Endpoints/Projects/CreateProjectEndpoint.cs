using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PulseTrack.Application.Features.Projects.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.Projects;

namespace PulseTrack.Api.Endpoints.Projects
{
    public class CreateProjectEndpoint : Endpoint<CreateProjectRequest>
    {
        private readonly IMediator _mediator;

        public CreateProjectEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateProjectRequest req, CancellationToken ct)
        {
            Project project = await _mediator.Send(
                new CreateProjectCommand(req.Name, req.Color, req.Icon, req.IsInbox),
                ct
            );

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                project,
                cancellationToken: ct
            );
        }
    }
}
