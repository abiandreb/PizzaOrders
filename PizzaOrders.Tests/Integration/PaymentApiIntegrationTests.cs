using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for Payment API: process payment for orders.
/// </summary>
[Category("Integration")]
public class PaymentApiIntegrationTests : IntegrationTestBase
{
    [SetUp]
    public async Task SetUp()
    {
        await EnsureProductExists();
    }

    #region Pay

    [Test]
    public async Task Pay_ValidOrder_ReturnsOk()
    {
        // Arrange — create an order with PaymentPending status
        var orderId = await SeedPaymentPendingOrder(25.00m);

        // Act
        var response = await Client.PostAsJsonAsync("/api/payment/pay",
            new { orderId, amount = 25.00m });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var result = await response.Content.ReadFromJsonAsync<PaymentResultDto>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.PaymentId, Is.GreaterThan(0));
        Assert.That(result.Status, Is.EqualTo("Paid"));
        Assert.That(result.TransactionId, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task Pay_AmountMismatch_ReturnsBadRequest()
    {
        // Arrange
        var orderId = await SeedPaymentPendingOrder(20.00m);

        // Act
        var response = await Client.PostAsJsonAsync("/api/payment/pay",
            new { orderId, amount = 10.00m }); // wrong amount

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Pay_NonExistentOrder_ReturnsBadRequest()
    {
        // Act
        var response = await Client.PostAsJsonAsync("/api/payment/pay",
            new { orderId = 99999, amount = 10.00m });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    #endregion

    #region Helpers

    private async Task<int> SeedPaymentPendingOrder(decimal totalPrice)
    {
        var orderId = 0;
        await ExecuteDbContextAsync(async context =>
        {
            var order = new OrderEntity
            {
                TotalPrice = totalPrice,
                Status = OrderStatus.PaymentPending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new()
                    {
                        ProductId = 1,
                        Quantity = 1,
                        ItemPrice = totalPrice,
                        TotalPrice = totalPrice,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                }
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            orderId = order.Id;
        });
        return orderId;
    }

    private async Task EnsureProductExists()
    {
        await ExecuteDbContextAsync(async context =>
        {
            if (!context.Products.Any(p => p.Id == 1))
            {
#pragma warning disable CS0618
                context.Products.Add(new ProductEntity
                {
                    Name = "Payment Test Pizza",
                    Description = "Pizza for payment tests",
                    BasePrice = 10.00m,
                    HasToppings = false,
                    ProductType = ProductType.Pizza,
                    ImageUrl = "payment-test.jpg"
                });
#pragma warning restore CS0618
                await context.SaveChangesAsync();
            }
        });
    }

    private class PaymentResultDto
    {
        public int PaymentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }

    #endregion
}
