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
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAppContext(IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<UserEntity, RoleEntity>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        
            return services;
        }

        public IServiceCollection AddApplicationServices()
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
        
            return services;
        }
    }
}