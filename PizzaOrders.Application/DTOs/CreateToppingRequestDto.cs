using System.ComponentModel.DataAnnotations;

namespace PizzaOrders.Application.DTOs;

public class CreateToppingRequestDto
{
    [Required(ErrorMessage = "Topping name is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string? Description { get; set; }

    [Range(0, 10000, ErrorMessage = "Stock must be between 0 and 10000")]
    public int Stock { get; set; }

    [Required]
    [Range(0, 100, ErrorMessage = "Price must be between 0 and 100")]
    public decimal Price { get; set; }
}
