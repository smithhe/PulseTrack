using Microsoft.AspNetCore.Builder;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PulseTrack.Infrastructure.Persistence;
using PulseTrack.Infrastructure.Persistence;
using MediatR;
using PulseTrack.Application.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// EF Core Postgres
string connectionString = builder.Configuration.GetConnectionString("Default")
                          ?? "Host=pulsetracker-postgres;Port=5432;Database=pulsetrack;Username=postgres;Password=postgres";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Health checks
builder.Services.AddHealthChecks().AddNpgSql(connectionString);

// FastEndpoints
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Infrastructure + MediatR
builder.Services.AddPersistenceInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>());

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api/v1");
app.UseSwaggerGen();
app.MapControllers();

app.Run();
