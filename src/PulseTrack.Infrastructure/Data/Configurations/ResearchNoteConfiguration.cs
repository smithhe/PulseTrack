using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseTrack.Domain.Entities;
using PulseTrack.Domain.Enums;

namespace PulseTrack.Infrastructure.Data.Configurations;

internal sealed class ResearchNoteConfiguration : IEntityTypeConfiguration<ResearchNote>
{
    public void Configure(EntityTypeBuilder<ResearchNote> builder)
    {
        builder.ToTable("ResearchNotes");

        builder.HasKey(note => note.Id);
        builder.Property(note => note.Id).ValueGeneratedNever();

        builder.Property(note => note.TopicId)
            .IsRequired();

        builder.Property(note => note.Kind)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(note => note.ContentMarkdown)
            .IsRequired()
            .HasMaxLength(4000)
            .HasColumnType("nvarchar(max)");

        builder.Property(note => note.LinkedWorkItemId);

        builder.Property(note => note.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(note => note.UpdatedAtUtc)
            .HasPrecision(0);

        builder.Property(note => note.RowVersion)
            .IsRowVersion();

        builder.HasOne<WorkItem>()
            .WithMany()
            .HasForeignKey(note => note.LinkedWorkItemId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(note => new { note.TopicId, note.Kind });
    }
}

