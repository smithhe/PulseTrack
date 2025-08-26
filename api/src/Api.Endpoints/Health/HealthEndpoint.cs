using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;

namespace PulseTrack.Api.Endpoints.Health
{
    /// <summary>
    /// Health check endpoint for monitoring service availability
    /// </summary>
    public record HealthResponse(string Status);

    /// <summary>
    /// Endpoint for checking the health status of the PulseTrack API service
    /// </summary>
    public class HealthEndpoint : EndpointWithoutRequest<HealthResponse>
    {
        /// <summary>
        /// Configures the health endpoint with GET method and anonymous access
        /// </summary>
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/health");
            AllowAnonymous();
        }

        /// <summary>
        /// Handles the health check request and returns the service status
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        public override async Task HandleAsync(CancellationToken ct)
        {
            try
            {
                await Send.OkAsync(new HealthResponse("ok"), ct);
            }
            catch (Exception)
            {
                await Send.ResponseAsync(
                    new HealthResponse("Unexpected Error Occurred"),
                    (int)HttpStatusCode.InternalServerError,
                    ct
                );
            }
        }
    }
}
