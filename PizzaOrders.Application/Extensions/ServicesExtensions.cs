using Microsoft.Extensions.DependencyInjection;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Services;

namespace PizzaOrders.Application.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPizzaService, PizzaService>();
        
        return services;
    }
}