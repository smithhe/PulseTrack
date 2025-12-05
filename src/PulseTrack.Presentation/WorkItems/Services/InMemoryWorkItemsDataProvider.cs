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
                Tags: new[] { "ux", "focus" }),

            new WorkItemListItem(
                Id: Guid.Parse("e9146d3d-4f9d-40d7-9d75-2b0b37ba1234"),
                Key: "OPS-1105",
                Title: "Consolidate notification settings",
                ProjectName: "PulseTrack Desktop",
                FeatureName: "Workspace Settings",
                OwnerDisplayName: "Noah Fields",
                Status: WorkItemStatus.Ready,
                Priority: WorkItemPriority.Medium,
                CreatedAtUtc: now.AddDays(-3),
                DueAtUtc: now.AddDays(11),
                CompletedAtUtc: null,
                Summary: "Map all notification toggles into a single panel so users can disable channels more easily.",
                Tags: new[] { "settings", "notifications" }),

            new WorkItemListItem(
                Id: Guid.Parse("a5cf7c51-5647-462e-9777-3a079c010a1f"),
                Key: "API-915",
                Title: "Expose aggregated readiness score",
                ProjectName: "PulseTrack Platform",
                FeatureName: "Insights API",
                OwnerDisplayName: "Marisol Vega",
                Status: WorkItemStatus.InProgress,
                Priority: WorkItemPriority.High,
                CreatedAtUtc: now.AddDays(-7),
                DueAtUtc: now.AddDays(1),
                CompletedAtUtc: null,
                Summary: "Combine HRV, sleep efficiency, and training load into a single readiness metric with weights from the data team.",
                Tags: new[] { "insights", "readiness" }),

            new WorkItemListItem(
                Id: Guid.Parse("b4d82177-8d52-4e47-90c0-12e3fb0e9876"),
                Key: "MOB-1932",
                Title: "Android biometric fast-path",
                ProjectName: "PulseTrack Mobile",
                FeatureName: "Auth & Security",
                OwnerDisplayName: "Priya Sundar",
                Status: WorkItemStatus.Blocked,
                Priority: WorkItemPriority.High,
                CreatedAtUtc: now.AddDays(-8),
                DueAtUtc: now.AddDays(4),
                CompletedAtUtc: null,
                Summary: "Play Store review flagged fallback PIN flow; waiting for legal sign-off on updated copy.",
                Tags: new[] { "android", "auth" }),

            new WorkItemListItem(
                Id: Guid.Parse("d5adf6e5-3fbe-4b29-a5b4-1b16eb4e4567"),
                Key: "DATA-655",
                Title: "Hydrate QA environment with anonymized logs",
                ProjectName: "PulseTrack Platform",
                FeatureName: "Data Pipeline",
                OwnerDisplayName: "Riley Owens",
                Status: WorkItemStatus.InProgress,
                Priority: WorkItemPriority.Medium,
                CreatedAtUtc: now.AddDays(-2),
                DueAtUtc: now.AddDays(6),
                CompletedAtUtc: null,
                Summary: "Need synthetic but realistic log bursts to exercise queue drains during load tests.",
                Tags: new[] { "pipeline", "qa", "observability" }),

            new WorkItemListItem(
                Id: Guid.Parse("f15a3d79-559b-41c9-bf12-0c6ded19c5e8"),
                Key: "OPS-1066",
                Title: "Coach assignment quick actions",
                ProjectName: "PulseTrack Desktop",
                FeatureName: "Work Management",
                OwnerDisplayName: "Harold Patel",
                Status: WorkItemStatus.Backlog,
                Priority: WorkItemPriority.Low,
                CreatedAtUtc: now.AddDays(-1),
                DueAtUtc: null,
                CompletedAtUtc: null,
                Summary: "Add hover action row at the end of each backlog card to assign or re-route without opening the detail sheet.",
                Tags: new[] { "desktop", "coaching" }),

            new WorkItemListItem(
                Id: Guid.Parse("cb8df1f9-86e2-4215-a65c-90962e7d3ab3"),
                Key: "MOB-1954",
                Title: "Night mode breathing exercise",
                ProjectName: "PulseTrack Mobile",
                FeatureName: "Focus Sessions",
                OwnerDisplayName: "Lena McCarthy",
                Status: WorkItemStatus.InReview,
                Priority: WorkItemPriority.Medium,
                CreatedAtUtc: now.AddDays(-5),
                DueAtUtc: now.AddDays(-1),
                CompletedAtUtc: null,
                Summary: "Adds warm color palette, haptic cues, and adaptive duration for evening breathing routines.",
                Tags: new[] { "mobile", "focus", "night-mode" }),

            new WorkItemListItem(
                Id: Guid.Parse("0e3b56ea-24f2-4c01-9dce-0ceaf8c3c52a"),
                Key: "API-930",
                Title: "Webhook signature rotation tooling",
                ProjectName: "PulseTrack Platform",
                FeatureName: "Integrations",
                OwnerDisplayName: "Jordan Singh",
                Status: WorkItemStatus.Ready,
                Priority: WorkItemPriority.Medium,
                CreatedAtUtc: now.AddDays(-10),
                DueAtUtc: now.AddDays(8),
                CompletedAtUtc: null,
                Summary: "Need migration command + docs so partners can rotate secrets without downtime.",
                Tags: new[] { "api", "security" }),

            new WorkItemListItem(
                Id: Guid.Parse("5f67c2c5-77a2-4c4b-8c17-b0a5f20a4eda"),
                Key: "OPS-1120",
                Title: "Desktop telemetry opt-in banner",
                ProjectName: "PulseTrack Desktop",
                FeatureName: "Workspace Settings",
                OwnerDisplayName: "Noah Fields",
                Status: WorkItemStatus.Ready,
                Priority: WorkItemPriority.Low,
                CreatedAtUtc: now.AddDays(-15),
                DueAtUtc: now.AddDays(-3),
                CompletedAtUtc: null,
                Summary: "Legal wants explicit opt-ins per workspace rather than global preference. Copy ready.",
                Tags: new[] { "compliance", "desktop" }),

            new WorkItemListItem(
                Id: Guid.Parse("6c67263f-8703-4aab-9a8a-b9cab4473def"),
                Key: "DATA-672",
                Title: "Research topic clustering spike",
                ProjectName: "PulseTrack Platform",
                FeatureName: "Research Hub",
                OwnerDisplayName: "Priya Sundar",
                Status: WorkItemStatus.Backlog,
                Priority: WorkItemPriority.Medium,
                CreatedAtUtc: now.AddDays(-13),
                DueAtUtc: null,
                CompletedAtUtc: null,
                Summary: "Investigate using embeddings to group similar research topics before we build the backlog planner.",
                Tags: new[] { "research", "ml", "spike" })
        };
    }
}

