using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Mappers;
using PizzaOrders.Domain.Entities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class OrderService(AppDbContext dbContext) : IOrderService
{
    public async Task<OrderObject> CreateOrder(CreateOrderDto dto)
    {
        var user = await GetUser(dto.UserId);

        var pizzas = await GetPizzas(dto);

        var orderItems = new List<OrderItem>();

        foreach (var item in dto.Items)
        {
            var pizza = pizzas.SingleOrDefault(x => x.Id == item.PizzaId);

            if (pizza != null)
                orderItems.Add(
                    new OrderItem
                    {
                        PizzaId = pizza.Id,
                        Quantity = item.Quantity,
                        ItemPrice = pizza.Price
                    });
        }

        var totalPrice = orderItems.Sum(x => x.ItemPrice * x.Quantity);

        var orderModel = new Order
        {
            Status = OrderStatus.New,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            OrderItems = orderItems,
            TotalPrice = totalPrice
        };

        await dbContext.Orders.AddAsync(orderModel);

        await dbContext.SaveChangesAsync();

        return orderModel.ToOrderDto();
    }

    public async Task<List<OrderObject>> GetOrders()
    {
        var ordersModels = await dbContext.Orders.ToListAsync();

        return ordersModels.Select(x => x.ToOrderDto()).ToList();
    }

    private async Task<List<Pizza>> GetPizzas(CreateOrderDto dto)
    {
        var pizzaIds = dto.Items.Select(x => x.PizzaId);

        var pizzas = await dbContext.Pizzas.Where(x => pizzaIds.Contains(x.Id)).ToListAsync();

        return pizzas.Count != dto.Items.Count
            ? throw new InvalidOperationException("Pizza not found")
            : pizzas.ToList();
    }

    private async Task<ApplicationUser> GetUser(string userId)
    {
        var userModel = await dbContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id.Equals(userId));

        return userModel ?? throw new InvalidOperationException("User not found");
    }
}