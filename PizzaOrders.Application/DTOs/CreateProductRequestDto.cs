using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Application.DTOs
{
    public class CreateProductRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public bool HasToppings { get; set; }
        public ProductType ProductType { get; set; }
        public string ImageUrl { get; set; }
        public ProductProperties? Properties { get; set; }
    }
}
