using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Data.Configurations;

internal sealed class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
{
    public void Configure(EntityTypeBuilder<TimeEntry> builder)
    {
        builder.ToTable("TimeEntries");

        builder.HasKey(entry => entry.Id);
        builder.Property(entry => entry.Id).ValueGeneratedNever();

        builder.Property(entry => entry.WorkItemId)
            .IsRequired();

        builder.Property(entry => entry.StartUtc)
            .HasPrecision(0)
            .IsRequired();

        builder.Property(entry => entry.EndUtc)
            .HasPrecision(0)
            .IsRequired();

        builder.Property(entry => entry.Source)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(entry => entry.Notes)
            .HasMaxLength(1024);

        builder.Property(entry => entry.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(entry => entry.UpdatedAtUtc)
            .HasPrecision(0);

        builder.Property(entry => entry.RowVersion)
            .IsRowVersion();

        builder.Ignore(entry => entry.Duration);

        builder.HasOne<WorkItem>()
            .WithMany()
            .HasForeignKey(entry => entry.WorkItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(entry => new { entry.WorkItemId, entry.StartUtc });
    }
}

