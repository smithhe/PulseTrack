using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Data.Configurations;

internal sealed class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private static readonly ValueComparer<HashSet<string>> TagsComparer = new(
        (left, right) =>
            ReferenceEquals(left, right) ||
            (left != null && right != null && left.SetEquals(right)),
        tags => tags != null ? ComputeTagsHash(tags) : 0,
        tags => tags != null
            ? new HashSet<string>(tags, StringComparer.OrdinalIgnoreCase)
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase));

    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable("WorkItems");

        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedNever();

        builder.Property(item => item.ProjectId).IsRequired();

        builder.Property(item => item.FeatureId);
        builder.Property(item => item.OwnerId);

        builder.Property(item => item.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(item => item.DescriptionMarkdown)
            .HasColumnType("nvarchar(max)");

        builder.Property(item => item.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(item => item.Priority)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(item => item.EstimatePoints)
            .HasPrecision(9, 2);

        builder.Property(item => item.DueAtUtc)
            .HasPrecision(0);

        builder.Property(item => item.CompletedAtUtc)
            .HasPrecision(0);

        builder.Property(item => item.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(item => item.UpdatedAtUtc)
            .HasPrecision(0);

        builder.Property(item => item.RowVersion)
            .IsRowVersion();

        var converter = new ValueConverter<HashSet<string>, string>(
            tags => JsonSerializer.Serialize(tags, JsonOptions),
            json => string.IsNullOrWhiteSpace(json)
                ? new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                : JsonSerializer.Deserialize<HashSet<string>>(json, JsonOptions) ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase));

        builder.Property<HashSet<string>>("_tags")
            .HasColumnName("Tags")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasConversion(converter)
            .Metadata.SetValueComparer(TagsComparer);
        builder.Ignore(item => item.Tags);

        builder.HasOne<Feature>()
            .WithMany()
            .HasForeignKey(item => item.FeatureId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<TeamMember>()
            .WithMany()
            .HasForeignKey(item => item.OwnerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(item => item.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(item => new { item.ProjectId, item.Status });
        builder.HasIndex(item => item.OwnerId);
    }

    private static int ComputeTagsHash(HashSet<string> tags)
    {
        if (tags.Count == 0)
        {
            return 0;
        }

        var hash = new HashCode();
        foreach (var tag in tags)
        {
            hash.Add(tag, StringComparer.OrdinalIgnoreCase);
        }

        return hash.ToHashCode();
    }
}

