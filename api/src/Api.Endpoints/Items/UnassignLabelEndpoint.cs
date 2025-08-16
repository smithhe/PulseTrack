using System;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using PulseTrack.Application.Features.Labels.Commands;
using PulseTrack.Shared.Requests.Items;

namespace PulseTrack.Api.Endpoints.Items;

public class UnassignLabelEndpoint : Endpoint<UnassignLabelRequest>
{
    private readonly IMediator _mediator;

    public UnassignLabelEndpoint(IMediator mediator)
    {
        this._mediator = mediator;
    }

    public override void Configure()
    {
        this.Verbs(Http.DELETE);
        this.Routes("/items/{id:guid}/labels");
        this.AllowAnonymous();
    }

    public override async Task HandleAsync(UnassignLabelRequest req, CancellationToken ct)
    {
        string? idStr = this.HttpContext.Request.RouteValues["id"]?.ToString();
        if (!Guid.TryParse(idStr, out Guid id))
        {
            this.HttpContext.Response.StatusCode = 404;
            return;
        }

        await this._mediator.Send(new UnassignLabelCommand(id, req.LabelId), ct);
        this.HttpContext.Response.StatusCode = 204;
    }
}
