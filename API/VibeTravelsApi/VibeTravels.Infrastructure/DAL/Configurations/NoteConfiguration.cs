using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects.Notes;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Infrastructure.DAL.Configurations;

internal sealed class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(e => e.Value, value => new NoteId(value));

        builder.Property(e => e.UserId)
            .HasConversion(e => e.Value, value => new UserId(value))
            .IsRequired();
        
        builder.HasIndex(e => new { e.Id, e.UserId }).IsUnique();

        builder.Property(e => e.Title)
            .HasConversion(e => e.Value, value => new NoteTitle(value))
            .IsRequired();

        builder.Property(e => e.Location)
            .HasConversion(e => e.Value, value => new NoteLocation(value))
            .IsRequired();

        builder.Property(e => e.Body)
            .HasConversion(e => e.Value, value => new NoteBody(value))
            .HasColumnName("body_md")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(e => e.DeletedAt);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
}