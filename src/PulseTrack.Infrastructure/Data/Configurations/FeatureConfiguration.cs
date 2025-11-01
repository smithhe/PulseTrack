using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Data.Configurations;

internal sealed class FeatureConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        builder.ToTable("Features");

        builder.HasKey(feature => feature.Id);
        builder.Property(feature => feature.Id).ValueGeneratedNever();

        builder.Property(feature => feature.ProjectId)
            .IsRequired();

        builder.Property(feature => feature.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(feature => feature.IsArchived)
            .IsRequired();

        builder.Property(feature => feature.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(feature => feature.UpdatedAtUtc)
            .HasPrecision(0);

        builder.Property(feature => feature.RowVersion)
            .IsRowVersion();
    }
}

