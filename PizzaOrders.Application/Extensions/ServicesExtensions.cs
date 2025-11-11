using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddAppContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<UserEntity, RoleEntity>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        
        return services;
    }
    
}