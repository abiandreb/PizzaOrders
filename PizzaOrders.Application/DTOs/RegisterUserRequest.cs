using System.ComponentModel.DataAnnotations;
using PizzaOrders.Domain;

namespace PizzaOrders.Application.DTOs;

public class RegisterUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; init; }

    [Required]
    [MinLength(6)]
    public string Password { get; init; }

    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Country { get; init; }
    public string? PostalCode { get; init; }

    public string Role { get; init; } = UserRolesConstants.UserRole;
}