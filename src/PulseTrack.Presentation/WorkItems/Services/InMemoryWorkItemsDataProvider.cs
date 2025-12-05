using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PulseTrack.Domain.Enums;
using PulseTrack.Presentation.WorkItems.Models;

namespace PulseTrack.Presentation.WorkItems.Services;

/// <summary>
/// Simple in-memory provider that returns a curated list of demo work items.
/// </summary>
public sealed class InMemoryWorkItemsDataProvider : IWorkItemsDataProvider
{
    private static readonly IReadOnlyList<WorkItemListItem> Items = CreateSeedData();

    public async Task<IReadOnlyList<WorkItemListItem>> GetWorkItemsAsync(CancellationToken cancellationToken)
    {
        // Simulate async work so loading indicators are exercised.
        await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken).ConfigureAwait(true);
        return Items;
    }

    private static IReadOnlyList<WorkItemListItem> CreateSeedData()
    {
        DateTime now = DateTime.UtcNow;
        return new[]
        {
            new WorkItemListItem(
                Id: Guid.Parse("6b2deab8-24f7-4dd4-9f32-8a56fb241a11"),
                Key: "OPS-1042",
                Title: "Add pulse trend panel to dashboard",
                ProjectName: "PulseTrack Desktop",
                FeatureName: "Home Dashboard",
                OwnerDisplayName: "Harold Patel",
                Status: WorkItemStatus.InProgress,
                Priority: WorkItemPriority.High,
                CreatedAtUtc: now.AddDays(-9),
                DueAtUtc: now.AddDays(2),
                CompletedAtUtc: null,
                Summary: "Visualize rolling 7-day averages for heart rate and stress so coaches can see trends quickly.",
                Tags: new[] { "dashboard", "insights" }),

            new WorkItemListItem(
                Id: Guid.Parse("b0d237e6-7d9a-4c62-8f0e-c2c3ab47a21f"),
                Key: "MOB-1890",
                Title: "Offline sync queue resiliency",
                ProjectName: "PulseTrack Mobile",
                FeatureName: "Sync Engine",
                OwnerDisplayName: "Lena McCarthy",
                Status: WorkItemStatus.InProgress,
                Priority: WorkItemPriority.Critical,
                CreatedAtUtc: now.AddDays(-4),
                DueAtUtc: now.AddDays(5),
                CompletedAtUtc: null,
                Summary: "Queue should survive device restarts and retries need exponential backoff.",
                Tags: new[] { "sync", "mobile", "reliability" }),

            new WorkItemListItem(
                Id: Guid.Parse("be57a434-3840-4f3f-8872-6a52cb6772bb"),
                Key: "API-872",
                Title: "Generate coach insight digest",
                ProjectName: "PulseTrack Platform",
                FeatureName: "Insights API",
                OwnerDisplayName: "Marisol Vega",
                Status: WorkItemStatus.Blocked,
                Priority: WorkItemPriority.High,
                CreatedAtUtc: now.AddDays(-12),
                DueAtUtc: now.AddDays(3),
                CompletedAtUtc: null,
                Summary: "Blocked waiting on scoring model 2.4 from the research team.",
                Tags: new[] { "insights", "ml" }),

            new WorkItemListItem(
                Id: Guid.Parse("b3576fe0-8fd0-4052-9f73-79b30bb1cb65"),
                Key: "OPS-1030",
                Title: "Add hover & dark polish to backlog screen",
                ProjectName: "PulseTrack Desktop",
                FeatureName: "Work Management",
                OwnerDisplayName: "Jordan Singh",
                Status: WorkItemStatus.InReview,
                Priority: WorkItemPriority.Medium,
                CreatedAtUtc: now.AddDays(-6),
                DueAtUtc: null,
                CompletedAtUtc: null,
                Summary: "Ensure the backlog list follows updated design tokens in dark mode.",
                Tags: new[] { "desktop", "ux" }),

            new WorkItemListItem(
                Id: Guid.Parse("c2f5086f-7102-4f99-8f1a-3e7c34e52eef"),
                Key: "DATA-642",
                Title: "Normalize sleep sample ingestion",
                ProjectName: "PulseTrack Platform",
                FeatureName: "Data Pipeline",
                OwnerDisplayName: null,
                Status: WorkItemStatus.Ready,
                Priority: WorkItemPriority.Medium,
                CreatedAtUtc: now.AddDays(-1),
                DueAtUtc: now.AddDays(9),
                CompletedAtUtc: null,
                Summary: "Need to map Garmin & Oura payloads into unified schema and flag anomalies.",
                Tags: new[] { "pipeline", "sleep" }),

            new WorkItemListItem(
                Id: Guid.Parse("f637c02f-695f-4ad1-a98b-7b367ba1f4dd"),
                Key: "OPS-1004",
                Title: "Improve focus timer UX",
                ProjectName: "PulseTrack Desktop",
                FeatureName: "Focus Sessions",
                OwnerDisplayName: "Lena McCarthy",
                Status: WorkItemStatus.Done,
                Priority: WorkItemPriority.Low,
                CreatedAtUtc: now.AddDays(-20),
                DueAtUtc: now.AddDays(-12),
                CompletedAtUtc: now.AddDays(-10),
                Summary: "Added gentle haptics and summarized session stats in the completion toast.",
                Tags: new[] { "ux", "focus" })
        };
    }
}

