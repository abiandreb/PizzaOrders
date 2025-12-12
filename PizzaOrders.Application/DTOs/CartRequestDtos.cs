using System.Collections.Generic;

namespace PizzaOrders.Application.DTOs
{
    public class CartItemRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public List<int>? ToppingIds { get; set; }
    }

    public class CartItemUpdateRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
