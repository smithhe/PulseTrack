using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PulseTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        // Infrastructure registrations (e.g., EF Core, repositories, external services) will be added here.
        return services;
    }
}

