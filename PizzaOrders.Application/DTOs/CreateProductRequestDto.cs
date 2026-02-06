using System.ComponentModel.DataAnnotations;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Application.DTOs;

public class CreateProductRequestDto
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 1000, ErrorMessage = "Base price must be between 0.01 and 1000")]
    public decimal BasePrice { get; set; }

    public bool HasToppings { get; set; }

    [Required]
    [EnumDataType(typeof(ProductType), ErrorMessage = "Invalid product type")]
    public ProductType ProductType { get; set; }

    [Url(ErrorMessage = "Invalid image URL format")]
    public string? ImageUrl { get; set; }

    public ProductProperties? Properties { get; set; }
}
