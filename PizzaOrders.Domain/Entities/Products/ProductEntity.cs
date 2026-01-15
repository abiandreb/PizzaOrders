using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PizzaOrders.Domain.Common;

namespace PizzaOrders.Domain.Entities.Products;

public class ProductEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool HasToppings { get; set; }
    public decimal BasePrice { get; set; }

    [Obsolete("Use ProductImage instead. This field is kept for backward compatibility.")]
    public string ImageUrl { get; set; }

    public ProductType ProductType { get; set; }

    public ProductProperties? ProductProperties { get; set; }

    /// <summary>
    /// Product images in multiple sizes (thumbnail, medium, full).
    /// Stored as an owned entity.
    /// </summary>
    public ProductImage? ProductImage { get; set; }
}