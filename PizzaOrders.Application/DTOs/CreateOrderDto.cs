namespace PizzaOrders.Application.DTOs;

public class CreateOrderDto
{
    public int UserId { get; set; }
    public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
}

public class CreateOrderItemDto
{
    public int PizzaId { get; set; }
    public int Quantity { get; set; }
}
