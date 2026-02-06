using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Application.DTOs;

public class ProductResponse
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public bool HasToppings { get; set; }
    public ProductType ProductType { get; set; }
    public string? ImageUrl { get; set; }
    public ProductImageResponse? ProductImage { get; set; }
    public ProductProperties? Properties { get; set; }
}

public class ProductImageResponse
{
    public string? ThumbnailUrl { get; set; }
    public string? MediumUrl { get; set; }
    public string? FullUrl { get; set; }
}