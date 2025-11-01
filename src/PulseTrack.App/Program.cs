using System;
using System.IO;
using Avalonia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PulseTrack.Application;
using PulseTrack.Infrastructure;
using PulseTrack.Presentation;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace PulseTrack.App;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        ConfigureConfiguration(builder);
        ConfigureSerilog(builder);
        ConfigureServices(builder.Services, builder.Configuration, builder.Environment.IsDevelopment());

        using var host = builder.Build();
        App.Initialize(host);

        host.Start();

        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "PulseTrack terminated unexpectedly");
            throw;
        }
        finally
        {
            host.StopAsync().GetAwaiter().GetResult();
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureConfiguration(HostApplicationBuilder builder)
    {
        builder.Configuration.Sources.Clear();
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables("PULSETRACK__");
    }

    private static void ConfigureSerilog(HostApplicationBuilder builder)
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var logDirectory = Path.Combine(appData, "PulseTrack", "logs");
        Directory.CreateDirectory(logDirectory);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "PulseTrack")
            .WriteTo.Console()
            .WriteTo.File(
                formatter: new JsonFormatter(),
                path: Path.Combine(logDirectory, "pulsetrack-.json"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 14,
                restrictedToMinimumLevel: LogEventLevel.Information,
                shared: true)
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger, dispose: false);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration, bool isDevelopmentEnv)
    {
        services.AddApplicationLayer();
        services.AddInfrastructureLayer(configuration, isDevelopmentEnv);
        services.AddPresentationLayer();

        services.AddSingleton<Views.MainWindow>();
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
