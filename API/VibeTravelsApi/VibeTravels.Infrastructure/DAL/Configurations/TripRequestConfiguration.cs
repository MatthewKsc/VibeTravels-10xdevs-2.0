using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.TripRequests;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Infrastructure.DAL.Configurations;

internal sealed class TripRequestConfiguration : IEntityTypeConfiguration<TripRequest>
{
    public void Configure(EntityTypeBuilder<TripRequest> builder)
    {
        builder.ToTable("trip_requests");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(e => e.Value, value => new TripRequestId(value))
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasConversion(e => e.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(e => e.NoteId)
            .HasConversion(e => e.Value, value => new NoteId(value))
            .IsRequired();

        builder.Property(e => e.Days)
            .HasConversion(e => e.Value, value => new TravelDays(value))
            .IsRequired();

        builder.Property(e => e.Travelers)
            .HasConversion(e => e.Value, value => new Travelers(value))
            .IsRequired();

        builder.Property(e => e.StartDate)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasIndex(e => new { e.Id, e.UserId }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.Note)
            .WithMany()
            .HasForeignKey(e => e.NoteId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
