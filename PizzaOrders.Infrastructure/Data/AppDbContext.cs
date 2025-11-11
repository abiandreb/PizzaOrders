using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Payment;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Extensions;

namespace PizzaOrders.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int, 
    UserClaimEntity,
    UserRoleEntity, 
    UserLoginEntity, 
    RoleClaimsEntity, 
    UserTokenEntity>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    { } 
    
    //DomainEntities
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<ProductToppingEntity> ProductToppings { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ToppingEntity> Toppings { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<OrderItemToppingEntity> OrderItemToppings { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }

    //AuthEntities 
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserClaimEntity> UserClaims { get; set; }
    public DbSet<UserRoleEntity> UserRoles { get; set; }
    public DbSet<UserTokenEntity> UserTokens { get; set; }
    public DbSet<RoleClaimsEntity> RoleClaims { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);   
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserEntity>().ToTable("Users");
        modelBuilder.Entity<UserClaimEntity>().ToTable("UserClaims");
        modelBuilder.Entity<UserRoleEntity>().ToTable("UserRoles");
        modelBuilder.Entity<UserTokenEntity>().ToTable("UserTokens");
        modelBuilder.Entity<RoleClaimsEntity>().ToTable("RoleClaims");
        modelBuilder.Entity<UserLoginEntity>().ToTable("UserLogins");
        modelBuilder.Entity<RoleEntity>().ToTable("Roles");
        modelBuilder.Entity<RefreshTokenEntity>().ToTable("RefreshTokens");
        
        modelBuilder.Entity<ProductToppingEntity>()
            .HasKey(pt => new { pt.ProductId, pt.ToppingId });

        modelBuilder.Entity<ProductToppingEntity>()
            .HasOne(p => p.Product)
            .WithMany(p => p.ProductToppings)
            .HasForeignKey(pt => pt.ProductId);
        
        modelBuilder.Entity<ProductToppingEntity>()
            .HasOne(pt => pt.Topping)
            .WithMany(t => t.ProductToppings)
            .HasForeignKey(pt => pt.ToppingId);
        
        modelBuilder.Entity<OrderEntity>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<PaymentEntity>(p => p.OrderId);
        
        modelBuilder.SeedDomainData();
    }
}