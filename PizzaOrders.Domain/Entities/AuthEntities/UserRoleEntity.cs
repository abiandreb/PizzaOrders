using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PizzaOrders.Domain.Entities.AuthEntities;

public class UserRoleEntity : IdentityUserRole<int>
{
    [Key]
    public int Id { get; set; }
}