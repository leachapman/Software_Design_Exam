using Software_Design_Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Software_Design_Exam.Data;

public class CafeDbContext : DbContext
{
    public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options) { }

    // one table per entity
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Order -> OrderItems Composition
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // EF should use the private list _items, not the read-only property
        modelBuilder.Entity<Order>()
            .Navigation(o => o.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // OrderItem -> Product: protect history
        modelBuilder.Entity<OrderItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product.Name -> obligatory field, mx 100 chars
        modelBuilder.Entity<Product>()
            .Property(p => p.Name).IsRequired().HasMaxLength(100);
    }
}