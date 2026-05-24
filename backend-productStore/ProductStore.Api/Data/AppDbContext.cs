using Microsoft.EntityFrameworkCore;
using ProductStore.Api.Entities;

namespace ProductStore.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}