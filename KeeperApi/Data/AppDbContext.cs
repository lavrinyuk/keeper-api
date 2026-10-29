using KeeperApi.Models;
using Microsoft.EntityFrameworkCore;

namespace KeeperApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<VisitorAction> VisitorActions => Set<VisitorAction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Visitor>()
            .ToTable(table => table.HasCheckConstraint(
                "CK_Visitors_TrustLevel",
                "\"TrustLevel\" >= 0 AND \"TrustLevel\" <= 100"));

        modelBuilder.Entity<VisitorAction>()
            .HasIndex(action => new { action.VisitorId, action.Action })
            .IsUnique();

        modelBuilder.Entity<VisitorAction>()
            .Property(action => action.Action)
            .HasMaxLength(30)
            .IsRequired();

        modelBuilder.Entity<VisitorAction>()
            .HasOne(action => action.Visitor)
            .WithMany(visitor => visitor.Actions)
            .HasForeignKey(action => action.VisitorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
