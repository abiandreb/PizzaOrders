namespace PizzaOrders.Application.Interfaces;

public interface IOrderNotificationService
{
    Task NotifyOrderStatusUpdatedAsync(int orderId, string status, DateTime updatedAt);
}
