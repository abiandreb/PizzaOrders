using System.ComponentModel.DataAnnotations;

namespace PizzaOrders.Application.DTOs;

public class CartItemRequestDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Valid ProductId is required")]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
    public int Quantity { get; set; }

    public List<int>? ToppingIds { get; set; }
}

public class CartItemUpdateRequestDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Valid ProductId is required")]
    public int ProductId { get; set; }

    [Required]
    [Range(0, 100, ErrorMessage = "Quantity must be between 0 and 100")]
    public int Quantity { get; set; }

    public List<int>? ToppingIds { get; set; }
}
