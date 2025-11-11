using Microsoft.EntityFrameworkCore;
using VibeTravels.Core.Entities;

namespace VibeTravels.Infrastructure.DAL;

public class VibeTravelsContext: DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Note> Notes { get; set; }
    public virtual DbSet<Profile> Profiles { get; set; }
    
    public VibeTravelsContext() { }

    public VibeTravelsContext(DbContextOptions<VibeTravelsContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.ApplyConfigurationsFromAssembly(assembly: GetType().Assembly);
    }
}