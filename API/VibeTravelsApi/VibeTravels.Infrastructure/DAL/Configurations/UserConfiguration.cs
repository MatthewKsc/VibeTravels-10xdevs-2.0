using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VibeTravels.Core.Entities;
using VibeTravels.Core.ValueObjects;
using VibeTravels.Core.ValueObjects.User;

namespace VibeTravels.Infrastructure.DAL.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).HasConversion(e => e.Value, value => new UserId(value));
        
        builder.Property(e => e.Email)
            .HasConversion(e => e.Value, value => new Email(value))
            .HasColumnType("citext")
            .IsRequired();
        builder.HasIndex(e => e.Email).IsUnique();

        builder.Property(e => e.Password)
            .HasConversion(e => e.Value, value => new HashedPassword(value))
            .IsRequired();
        
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt).IsRequired();
    }
}