using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Data.Configurations;

internal sealed class ResearchTopicConfiguration : IEntityTypeConfiguration<ResearchTopic>
{
    public void Configure(EntityTypeBuilder<ResearchTopic> builder)
    {
        builder.ToTable("ResearchTopics");

        builder.HasKey(topic => topic.Id);
        builder.Property(topic => topic.Id).ValueGeneratedNever();

        builder.Property(topic => topic.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(topic => topic.ProblemArea)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(topic => topic.Goal)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(topic => topic.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(topic => topic.UpdatedAtUtc)
            .HasPrecision(0);

        builder.Property(topic => topic.RowVersion)
            .IsRowVersion();

        builder.HasMany(topic => topic.Notes)
            .WithOne()
            .HasForeignKey(note => note.TopicId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(topic => topic.Notes)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

