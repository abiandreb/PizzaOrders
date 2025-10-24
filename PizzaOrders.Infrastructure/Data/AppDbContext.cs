using Microsoft.EntityFrameworkCore;
using PizzaOrders.Domain.Entities;

namespace PizzaOrders.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<RevokedAccessToken> RevokedAccessTokens { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pizza>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(x => x.ItemPrice)
            .HasPrecision(18, 2);
  
        modelBuilder.Entity<Order>()
            .Property(x => x.TotalPrice)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<OrderItem>()
            .HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId);

        modelBuilder.Entity<Order>()
            .HasOne(x => x.User)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<User>()
            .HasMany(x => x.Orders)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<User>()
            .HasData(
                new User
                {
                    Id = 1,
                    Name = "Andrii",
                    Email = "test@test.test",
                    Phone = "13432463",
                    Address = "Makowa",
                    PasswordHash = "null",
                    Roles = "Admin",
                    IsLocked = false,
                    CreatedAt = new DateTime(2025, 10, 18, 12, 45, 34, 123, DateTimeKind.Utc)
                });
        
        modelBuilder.Entity<Pizza>()
            .HasData(new() {
                    Id = 1,
                    Name = "Margarita",
                    Description = "Margarita",
                    Price = 10
                }, new Pizza()
                {
                    Id = 2,
                    Name = "Pepperoni",
                    Description = "Pepperoni",
                    Price = 15
                });
    }
}