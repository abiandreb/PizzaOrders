using Microsoft.AspNetCore.SignalR;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.API.Hubs;

public class SignalROrderNotificationService(IHubContext<OrderHub> hubContext) : IOrderNotificationService
{
    public async Task NotifyOrderStatusUpdatedAsync(int orderId, string status, DateTime updatedAt)
    {
        await hubContext.Clients.Group($"order-{orderId}")
            .SendAsync("OrderStatusUpdated", new { orderId, status, updatedAt });
    }
}
