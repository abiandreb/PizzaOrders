using Microsoft.AspNetCore.Identity;

namespace PizzaOrders.Domain.Entities.AuthEntities;

public class UserEntity : IdentityUser<int>
{
    public string? Address { get; set; }
    public bool IsGuest { get; set; } = true;
}