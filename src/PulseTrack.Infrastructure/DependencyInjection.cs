using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PulseTrack.Infrastructure.Data;

namespace PulseTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnv)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var connectionString = configuration.GetConnectionString("PulseTrackDb")
            ?? Environment.GetEnvironmentVariable("PULSETRACK__DB__CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("A connection string named 'PulseTrackDb' must be configured.");
        }

        services.AddDbContext<PulseTrackDbContext>((serviceProvider, options) =>
        {
            options.UseSqlServer(
                connectionString,
                sql =>
                {
                    sql.MigrationsAssembly(typeof(PulseTrackDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure();
                });

            if (isDevelopmentEnv)
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        return services;
    }
}

