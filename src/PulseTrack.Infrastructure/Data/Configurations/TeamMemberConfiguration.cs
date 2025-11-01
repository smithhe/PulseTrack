using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Infrastructure.Data.Configurations;

internal sealed class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder)
    {
        builder.ToTable("TeamMembers");

        builder.HasKey(member => member.Id);
        builder.Property(member => member.Id).ValueGeneratedNever();

        builder.Property(member => member.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(member => member.Role)
            .HasMaxLength(100);

        builder.Property(member => member.Email)
            .HasMaxLength(256);

        builder.Property(member => member.IsActive)
            .IsRequired();

        builder.Property(member => member.CreatedAtUtc)
            .HasPrecision(0);

        builder.Property(member => member.UpdatedAtUtc)
            .HasPrecision(0);

        builder.Property(member => member.RowVersion)
            .IsRowVersion();

        builder.HasIndex(member => member.Email)
            .HasFilter("[Email] IS NOT NULL");
    }
}

