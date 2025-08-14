using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace PulseTrack.Api.Endpoints.Health
{
    public record HealthResponse(string Status);

    public class HealthEndpoint : EndpointWithoutRequest<HealthResponse>
    {
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/health");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await HttpContext.Response.WriteAsJsonAsync(new HealthResponse("ok"), ct);
        }
    }
}


