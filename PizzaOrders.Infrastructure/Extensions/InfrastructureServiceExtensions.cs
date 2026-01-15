using Microsoft.Extensions.DependencyInjection;
using PizzaOrders.Domain.Interfaces;
using PizzaOrders.Infrastructure.Services;

namespace PizzaOrders.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddScoped<IImageStorageService, AzuriteImageStorageService>();
        return services;
    }
}
