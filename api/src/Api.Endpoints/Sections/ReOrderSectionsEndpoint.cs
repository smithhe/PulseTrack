using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Sections.Commands;
using PulseTrack.Shared.Requests.Sections;

namespace PulseTrack.Api.Endpoints.Sections
{
    public class ReOrderSectionsEndpoint : Endpoint<ReorderSectionsRequest>
    {
        private readonly IMediator _mediator;

        public ReOrderSectionsEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/projects/{projectId:guid}/reorder-sections");
            AllowAnonymous();
        }

        public override async Task HandleAsync(ReorderSectionsRequest req, CancellationToken ct)
        {
            string? pidStr = HttpContext.Request.RouteValues["projectId"]?.ToString();
            if (!Guid.TryParse(pidStr, out Guid projectId))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            await _mediator.Send(new ReorderSectionsCommand(projectId, req.OrderedSectionIds), ct);
            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                new { ok = true },
                cancellationToken: ct
            );
        }
    }
}
