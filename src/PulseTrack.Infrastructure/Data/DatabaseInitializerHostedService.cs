using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PulseTrack.Infrastructure.Data;

internal sealed class DatabaseInitializerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly InfrastructureEnvironment _environment;
    private readonly ILogger<DatabaseInitializerHostedService> _logger;

    public DatabaseInitializerHostedService(
        IServiceProvider serviceProvider,
        InfrastructureEnvironment environment,
        ILogger<DatabaseInitializerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _environment = environment;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        PulseTrackDbContext dbContext = scope.ServiceProvider.GetRequiredService<PulseTrackDbContext>();

        _logger.LogInformation("Applying database migrations...");
        await dbContext.Database.MigrateAsync(cancellationToken);

        if (_environment.IsDevelopment)
        {
            DevelopmentSeeder seeder = scope.ServiceProvider.GetRequiredService<DevelopmentSeeder>();
            await seeder.SeedAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

