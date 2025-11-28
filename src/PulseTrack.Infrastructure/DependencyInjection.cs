using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PulseTrack.Infrastructure.Contracts;
using PulseTrack.Infrastructure.Data;
using PulseTrack.Infrastructure.Repositories;

namespace PulseTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnv)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        string? connectionString = configuration.GetConnectionString("PulseTrackDb")
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

        services.AddScoped<DevelopmentSeeder>();
        services.AddSingleton(new InfrastructureEnvironment(isDevelopmentEnv));
        services.AddHostedService<DatabaseInitializerHostedService>();

        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();

        return services;
    }
}

