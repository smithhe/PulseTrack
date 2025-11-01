using Microsoft.EntityFrameworkCore;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Data;

public sealed class PulseTrackDbContext : DbContext
{
    public PulseTrackDbContext(DbContextOptions<PulseTrackDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Feature> Features => Set<Feature>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
    public DbSet<ResearchTopic> ResearchTopics => Set<ResearchTopic>();
    public DbSet<ResearchNote> ResearchNotes => Set<ResearchNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PulseTrackDbContext).Assembly);
    }
}

