using System;
using Microsoft.Extensions.DependencyInjection;

namespace PulseTrack.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Application layer services will be registered here as they are implemented.
        return services;
    }
}

