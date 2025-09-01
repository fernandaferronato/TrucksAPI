using Microsoft.EntityFrameworkCore;
using Trucks.Api.Models;

namespace Trucks.Api.Data;

public class TrucksDbContext : DbContext
{
    public TrucksDbContext(DbContextOptions<TrucksDbContext> options) : base(options) {}

    public DbSet<Truck> Trucks => Set<Truck>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Truck>()
            .HasIndex(t => t.ChassisId)
            .IsUnique();
    }
}