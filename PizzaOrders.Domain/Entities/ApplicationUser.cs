using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PizzaOrders.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    [MaxLength(256)]
    public string? Address { get; set; }
    public ICollection<Order>? Orders { get; set; } = new List<Order>();
}