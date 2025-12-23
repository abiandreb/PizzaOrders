using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Payment;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services
{
    public class PaymentService(AppDbContext context) : IPaymentService
    {
        public async Task<PaymentResponseDto> ProcessPayment(PaymentRequestDto paymentRequest)
        {
            var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == paymentRequest.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException("Order not found.");
            }

            if (order.TotalPrice != paymentRequest.Amount)
            {
                throw new InvalidOperationException("Payment amount does not match order total.");
            }

            var payment = new PaymentEntity
            {
                OrderId = order.Id,
                Amount = order.TotalPrice,
                Method = PaymentMethod.Online, // Mocked
                Status = PaymentStatus.Paid,    // Mocked as always successful
                TransactionId = Guid.NewGuid().ToString(), // Mocked
                Gateway = "MockPay",
                CreatedAt = DateTime.UtcNow,
                ConfirmedAt = DateTime.UtcNow
            };

            context.Payments.Add(payment);

            order.Status = OrderStatus.Paid;
            order.PaymentId = payment.Id;

            await context.SaveChangesAsync();

            return new PaymentResponseDto
            {
                PaymentId = payment.Id,
                Status = payment.Status.ToString(),
                TransactionId = payment.TransactionId
            };
        }
    }
}
