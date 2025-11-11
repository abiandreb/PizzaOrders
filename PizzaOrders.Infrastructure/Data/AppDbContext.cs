using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Domain.Entities.Orders;

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
    }
}