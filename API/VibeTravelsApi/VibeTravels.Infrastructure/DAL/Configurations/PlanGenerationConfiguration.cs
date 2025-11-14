using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.PlanGenerations;
using VibeTravels.Core.ValueObjects.TripRequests;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Infrastructure.DAL.Configurations;

internal sealed class PlanGenerationConfiguration : IEntityTypeConfiguration<PlanGeneration>
{
    public void Configure(EntityTypeBuilder<PlanGeneration> builder)
    {
        builder.ToTable("plan_generations");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(e => e.Value, value => new PlanGenerationId(value))
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasConversion(e => e.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(e => e.TripRequestId)
            .HasConversion(e => e.Value, value => new TripRequestId(value))
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(e => e.StartedAt)
            .IsRequired(false);

        builder.Property(e => e.FinishedAt)
            .IsRequired(false);

        builder.Property(e => e.InputPayload)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(e => e.OutputRaw)
            .IsRequired(false);

        builder.Property(e => e.ErrorMessage)
            .IsRequired(false);

        builder.HasIndex(e => new { e.Id, e.UserId }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.TripRequest)
            .WithMany()
            .HasForeignKey(e => e.TripRequestId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}

