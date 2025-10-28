using System.ComponentModel.DataAnnotations;

namespace PizzaOrders.Application.DTOs;

public class AuthLoginRequest
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}