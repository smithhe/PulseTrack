using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.DueDates.Commands;
using PulseTrack.Domain.Entities;
using PulseTrack.Shared.Requests.DueDates;

namespace PulseTrack.Api.Endpoints.Items
{
    public class UpsertDueDateEndpoint : Endpoint<UpsertDueDateRequest>
    {
        private readonly IMediator _mediator;

        public UpsertDueDateEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Verbs(Http.PUT);
            Routes("/items/{id:guid}/due-date");
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpsertDueDateRequest req, CancellationToken ct)
        {
            string? idStr = HttpContext.Request.RouteValues["id"]?.ToString();
            if (!Guid.TryParse(idStr, out Guid id))
            {
                HttpContext.Response.StatusCode = 404;
                return;
            }

            DueDate due = await _mediator.Send(
                new UpsertDueDateCommand(
                    id,
                    req.DateUtc,
                    req.Timezone,
                    req.IsRecurring,
                    req.RecurrenceType,
                    req.RecurrenceInterval,
                    req.RecurrenceCount,
                    req.RecurrenceEndUtc,
                    req.RecurrenceWeeks
                ),
                ct
            );

            HttpContext.Response.ContentType = "application/json";
            await JsonSerializer.SerializeAsync(
                HttpContext.Response.Body,
                due,
                cancellationToken: ct
            );
        }
    }
}
