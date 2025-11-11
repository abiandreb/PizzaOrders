using PizzaOrders.Domain;

namespace PizzaOrders.Application.DTOs;

public class RegisterUserRequest
{
    public string Email { get; }
    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string PhoneNumber { get; }
    public string Address { get; }
    public string City { get; }
    public string Country { get; }
    public string PostalCode { get; }
    public string Role { get; } = UserRolesConstants.UserRole;
}