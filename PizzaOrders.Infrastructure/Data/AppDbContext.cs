using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Domain.Entities;

namespace PizzaOrders.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    
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
            .HasOne(x => x.ApplicationUser)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(x => x.Orders)
            .WithOne(x => x.ApplicationUser)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "1",
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEGf2cepss0DRE4sIBea2pAfETanwltxm//I+SKgQFDBETUTmelVuLDDDfR2JVlLHbQ==",
                SecurityStamp = "a1b2c3d4-e5f6-7890-abcd-111111111111",
                ConcurrencyStamp = "b2c3d4e5-f6a7-890b-cdef-222222222222"
            },
            new ApplicationUser()
            {
                Id = "2",
                UserName = "user@example.com",
                NormalizedUserName = "USER@EXAMPLE.COM",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEOtLW74d8BO0kNYtQLicCfvlWOb0SQZWxfL4+s85B6wIxNPsOy1Z6ur8nmZudlQEeg==",
                SecurityStamp = "c3d4e5f6-a7b8-901c-def0-333333333333",
                ConcurrencyStamp = "d4e5f6a7-b8c9-012d-ef01-444444444444"
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