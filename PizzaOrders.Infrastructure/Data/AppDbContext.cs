using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
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
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ToppingEntity> Toppings { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
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
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
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

        modelBuilder.Entity<OrderEntity>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<PaymentEntity>(p => p.OrderId);

        modelBuilder
            .Entity<OrderItemEntity>()
            .Property(e => e.ItemModifiers)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<ItemModifiers>(v));

        modelBuilder
            .Entity<ProductEntity>()
            .Property(e => e.ProductProperties)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<ProductProperties>(v));
        
        modelBuilder.SeedDomainData();
    }
}