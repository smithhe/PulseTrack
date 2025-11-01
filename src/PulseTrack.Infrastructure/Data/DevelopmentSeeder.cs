using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PulseTrack.Domain.Entities;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Infrastructure.Data;

internal sealed class DevelopmentSeeder
{
    private readonly PulseTrackDbContext _dbContext;
    private readonly ILogger<DevelopmentSeeder> _logger;

    public DevelopmentSeeder(PulseTrackDbContext dbContext, ILogger<DevelopmentSeeder> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!await NeedsSeedingAsync(cancellationToken))
        {
            return;
        }

        _logger.LogInformation("Seeding development data...");

        var now = DateTime.UtcNow;

        var project = new Project(Guid.NewGuid(), "PulseTrack", "PTK", "#3B82F6", now);
        var deliveryFeature = project.AddFeature(Guid.NewGuid(), "Delivery Workflow", now);
        var analyticsFeature = project.AddFeature(Guid.NewGuid(), "Analytics Dashboard", now);

        var lead = new TeamMember(Guid.NewGuid(), "Harold TeamLead", "Engineering Lead", "harold@example.com", now);
        var developer = new TeamMember(Guid.NewGuid(), "Avery Dev", "Senior Developer", "avery@example.com", now);

        var backlogItem = new WorkItem(
            Guid.NewGuid(),
            project.Id,
            "Implement core navigation shell",
            WorkItemStatus.InProgress,
            WorkItemPriority.High,
            now);
        backlogItem.AssignFeature(deliveryFeature.Id, now);
        backlogItem.AssignOwner(developer.Id, now);
        backlogItem.UpdateDescription("Set up the navigation rail, workspace tabs, and responsive layout.", now);
        backlogItem.AddTag("ui", now);
        backlogItem.AddTag("shell", now);

        var researchTopic = new ResearchTopic(
            Guid.NewGuid(),
            "Deployment Throughput",
            "Release cadence",
            "Improve delivery frequency without increasing regressions",
            now);

        var researchNote = researchTopic.AddNote(
            Guid.NewGuid(),
            ResearchNoteKind.Observation,
            "Over the last three sprints we averaged 1.2 releases/week due to manual verification bottlenecks.",
            now,
            backlogItem.Id);

        var timeEntry = new TimeEntry(
            Guid.NewGuid(),
            backlogItem.Id,
            now.AddHours(-6),
            now.AddHours(-3),
            TimeEntrySource.Manual,
            "Initial shell layout implementation",
            now);

        _dbContext.Projects.Add(project);
        _dbContext.TeamMembers.AddRange(lead, developer);
        _dbContext.WorkItems.Add(backlogItem);
        _dbContext.TimeEntries.Add(timeEntry);
        _dbContext.ResearchTopics.Add(researchTopic);
        _dbContext.ResearchNotes.Add(researchNote);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Development data seeded successfully.");
    }

    private async Task<bool> NeedsSeedingAsync(CancellationToken cancellationToken)
    {
        var hasProjects = await _dbContext.Projects.AnyAsync(cancellationToken);
        return !hasProjects;
    }
}

