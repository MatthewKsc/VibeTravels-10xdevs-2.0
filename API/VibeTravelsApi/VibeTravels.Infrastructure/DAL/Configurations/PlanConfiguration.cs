using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeTravels.Core.Const;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.PlanGenerations;
using VibeTravels.Core.ValueObjects.Plans;
using VibeTravels.Core.ValueObjects.TripRequests;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Infrastructure.DAL.Configurations;

internal sealed class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.ToTable("plans");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(e => e.Value, value => new PlanId(value))
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasConversion(e => e.Value, value => new UserId(value))
            .IsRequired();

        builder.Property(e => e.TripRequestId)
            .HasConversion(e => e.Value, value => new TripRequestId(value))
            .IsRequired();

        builder.Property(e => e.PlanGenerationId)
            .HasConversion(e => e.Value, value => new PlanGenerationId(value))
            .IsRequired();

        builder.Property(e => e.StructureType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(e => e.DaysCount)
            .HasColumnName("days_count")
            .IsRequired(false);

        builder.Property(e => e.Content)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasDefaultValue(PlanStatus.NotGenerated)
            .IsRequired();

        builder.Property(e => e.DecisionReason)
            .IsRequired(false);

        builder.Property(e => e.DecisionAt)
            .IsRequired(false);

        builder.Property(e => e.AdjustedByUser)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.HasIndex(e => e.PlanGenerationId).IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.TripRequest)
            .WithMany()
            .HasForeignKey(e => e.TripRequestId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(e => e.PlanGeneration)
            .WithMany()
            .HasForeignKey(e => e.PlanGenerationId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Plans_DaysCount", "days_count IS NULL OR days_count > 0");
        });
    }
}

