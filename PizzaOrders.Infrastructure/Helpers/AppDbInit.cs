using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PizzaOrders.Domain;
using PizzaOrders.Domain.Entities;

namespace PizzaOrders.Infrastructure.Helpers;

public static class AppDbInit
{
    public static async Task SeedUserRoles(IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(UserRolesConstants.AdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(UserRolesConstants.AdminRole));
        }
        if (!await roleManager.RoleExistsAsync(UserRolesConstants.UserRole))
        {
            await roleManager.CreateAsync(new IdentityRole(UserRolesConstants.UserRole));
        }
    }
}