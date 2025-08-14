using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PulseTrack.BackgroundJobs.Worker;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

IHost host = builder.Build();
host.Run();
