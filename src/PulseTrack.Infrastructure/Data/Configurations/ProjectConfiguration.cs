using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Data.Configurations;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(project => project.Id);
        builder.Property(project => project.Id).ValueGeneratedNever();

        builder.Property(project => project.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(project => project.Key)
            .IsRequired()
            .HasMaxLength(8);

        builder.HasIndex(project => project.Key).IsUnique();

        builder.Property(project => project.Color)
            .HasMaxLength(9);

        builder.Property(project => project.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(project => project.UpdatedAtUtc)
            .HasPrecision(0);

        builder.Property(project => project.RowVersion)
            .IsRowVersion();

        builder.HasMany(project => project.Features)
            .WithOne()
            .HasForeignKey(feature => feature.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(project => project.Features)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

