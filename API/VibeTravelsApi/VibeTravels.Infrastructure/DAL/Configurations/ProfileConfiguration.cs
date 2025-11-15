using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.Profile;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Infrastructure.DAL.Configurations;

internal sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(e => e.Value, value => new ProfileId(value));

        builder.Property(e => e.UserId)
            .HasConversion(e => e.Value, value => new UserId(value))
            .IsRequired();

        builder.HasIndex(e => e.UserId).IsUnique();

        builder.Property(e => e.TravelStyle)
            .HasConversion(
                e => e != null ? e.Value : null,
                value => value != null ? new TravelStyle(value) : null)
            .HasColumnName("travel_style");

        builder.Property(e => e.AccommodationType)
            .HasConversion(
                e => e != null ? e.Value : null,
                value => value != null ? new AccommodationType(value) : null)
            .HasColumnName("accommodation_type");

        builder.Property(e => e.ClimateRegion)
            .HasConversion(
                e => e != null ? e.Value : null,
                value => value != null ? new ClimateRegion(value) : null)
            .HasColumnName("climate_region");

        builder.Property(e => e.CompletedAt)
            .HasColumnName("completed_at");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");
        
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Profile>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}

