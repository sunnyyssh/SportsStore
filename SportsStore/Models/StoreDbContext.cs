using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public sealed class StoreDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();

    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options) { }
}