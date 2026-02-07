using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class PaymentServiceTests
{
    private AppDbContext _dbContext = null!;
    private PaymentService _paymentService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _paymentService = new PaymentService(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    #region Error Cases

    [Test]
    public void ProcessPayment_OrderNotFound_ThrowsException()
    {
        // Arrange
        var request = new PaymentRequestDto { OrderId = 999, Amount = 10.0m };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.ProcessPayment(request));
    }

    [Test]
    public async Task ProcessPayment_AmountMismatch_ThrowsException()
    {
        // Arrange
        _dbContext.Orders.Add(new OrderEntity { Id = 1, TotalPrice = 20.0m, Status = OrderStatus.PaymentPending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        await _dbContext.SaveChangesAsync();

        var request = new PaymentRequestDto { OrderId = 1, Amount = 10.0m };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.ProcessPayment(request));
        Assert.That(ex!.Message, Does.Contain("does not match"));
    }

    #endregion

    #region Success Cases

    [Test]
    public async Task ProcessPayment_ValidRequest_ReturnsPaymentResponse()
    {
        // Arrange
        _dbContext.Orders.Add(new OrderEntity { Id = 1, TotalPrice = 25.00m, Status = OrderStatus.PaymentPending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        await _dbContext.SaveChangesAsync();

        var request = new PaymentRequestDto { OrderId = 1, Amount = 25.00m };

        // Act
        var result = await _paymentService.ProcessPayment(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Status, Is.EqualTo("Paid"));
        Assert.That(result.TransactionId, Is.Not.Null.And.Not.Empty);
        Assert.That(result.PaymentId, Is.GreaterThan(0));
    }

    [Test]
    public async Task ProcessPayment_UpdatesOrderStatusToPaid()
    {
        // Arrange
        _dbContext.Orders.Add(new OrderEntity { Id = 1, TotalPrice = 15.00m, Status = OrderStatus.PaymentPending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        await _dbContext.SaveChangesAsync();

        var request = new PaymentRequestDto { OrderId = 1, Amount = 15.00m };

        // Act
        await _paymentService.ProcessPayment(request);

        // Assert
        var order = await _dbContext.Orders.FindAsync(1);
        Assert.That(order!.Status, Is.EqualTo(OrderStatus.Paid));
    }

    [Test]
    public async Task ProcessPayment_CreatesPaymentEntity()
    {
        // Arrange
        _dbContext.Orders.Add(new OrderEntity { Id = 1, TotalPrice = 30.00m, Status = OrderStatus.PaymentPending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
        await _dbContext.SaveChangesAsync();

        var request = new PaymentRequestDto { OrderId = 1, Amount = 30.00m };

        // Act
        var result = await _paymentService.ProcessPayment(request);

        // Assert
        var payment = await _dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == 1);
        Assert.That(payment, Is.Not.Null);
        Assert.That(payment!.Amount, Is.EqualTo(30.00m));
        Assert.That(payment.Gateway, Is.EqualTo("MockPay"));
        Assert.That(payment.ConfirmedAt, Is.Not.Null);
    }

    #endregion
}
