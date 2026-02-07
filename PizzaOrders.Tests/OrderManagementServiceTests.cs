using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class OrderManagementServiceTests
{
    private AppDbContext _dbContext = null!;
    private Mock<ILogger<OrderManagementService>> _loggerMock = null!;
    private Mock<IOrderNotificationService> _notificationMock = null!;
    private OrderManagementService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _loggerMock = new Mock<ILogger<OrderManagementService>>();
        _notificationMock = new Mock<IOrderNotificationService>();
        _service = new OrderManagementService(_dbContext, _loggerMock.Object, _notificationMock.Object);

        SeedTestData();
    }

    private void SeedTestData()
    {
#pragma warning disable CS0618
        _dbContext.Products.Add(new ProductEntity
        {
            Id = 1,
            Name = "Margherita",
            Description = "Classic pizza",
            BasePrice = 10.00m,
            HasToppings = false,
            ProductType = ProductType.Pizza,
            ImageUrl = "margherita.jpg"
        });
#pragma warning restore CS0618

        _dbContext.Orders.AddRange(
            new OrderEntity
            {
                Id = 1,
                UserId = null,
                TotalPrice = 20.00m,
                Status = OrderStatus.Paid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new()
                    {
                        Id = 1, ProductId = 1, Quantity = 2, ItemPrice = 10.00m, TotalPrice = 20.00m,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    }
                }
            },
            new OrderEntity
            {
                Id = 2,
                UserId = null,
                TotalPrice = 10.00m,
                Status = OrderStatus.Completed,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new()
                    {
                        Id = 2, ProductId = 1, Quantity = 1, ItemPrice = 10.00m, TotalPrice = 10.00m,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    }
                }
            },
            new OrderEntity
            {
                Id = 3,
                UserId = null,
                TotalPrice = 10.00m,
                Status = OrderStatus.Cancelled,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new()
                    {
                        Id = 3, ProductId = 1, Quantity = 1, ItemPrice = 10.00m, TotalPrice = 10.00m,
                        CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
                    }
                }
            }
        );

        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    #region GetNextStatuses Tests

    [TestCase("New", new[] { "Cancelled" })]
    [TestCase("PaymentPending", new[] { "Cancelled" })]
    [TestCase("Paid", new[] { "Accepted", "Cancelled" })]
    [TestCase("Accepted", new[] { "Preparing", "Cancelled" })]
    [TestCase("Preparing", new[] { "Ready", "Cancelled" })]
    [TestCase("Ready", new[] { "Delivering" })]
    [TestCase("Delivering", new[] { "Delivered" })]
    [TestCase("Delivered", new[] { "Completed" })]
    [TestCase("Completed", new string[0])]
    [TestCase("Cancelled", new string[0])]
    [TestCase("Failed", new string[0])]
    public void GetNextStatuses_ReturnsCorrectTransitions(string currentStatus, string[] expected)
    {
        // Act
        var result = OrderManagementService.GetNextStatuses(currentStatus);

        // Assert
        Assert.That(result, Is.EqualTo(expected.ToList()));
    }

    [Test]
    public void GetNextStatuses_UnknownStatus_ReturnsEmpty()
    {
        // Act
        var result = OrderManagementService.GetNextStatuses("SomeUnknownStatus");

        // Assert
        Assert.That(result, Is.Empty);
    }

    #endregion

    #region GetAllOrdersAsync Tests

    [Test]
    public async Task GetAllOrdersAsync_ReturnsAllOrders_WithNextStatuses()
    {
        // Act
        var result = await _service.GetAllOrdersAsync();

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));

        var paidOrder = result.First(o => o.Id == 1);
        Assert.That(paidOrder.NextStatuses, Is.EqualTo(new List<string> { "Accepted", "Cancelled" }));

        var completedOrder = result.First(o => o.Id == 2);
        Assert.That(completedOrder.NextStatuses, Is.Empty);

        var cancelledOrder = result.First(o => o.Id == 3);
        Assert.That(cancelledOrder.NextStatuses, Is.Empty);
    }

    [Test]
    public async Task GetAllOrdersAsync_WithStatusFilter_ReturnsFilteredOrders()
    {
        // Act
        var result = await _service.GetAllOrdersAsync(OrderStatus.Paid);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Status, Is.EqualTo("Paid"));
    }

    #endregion

    #region GetOrderByIdAsync Tests

    [Test]
    public async Task GetOrderByIdAsync_ExistingOrder_ReturnsOrderWithNextStatuses()
    {
        // Act
        var result = await _service.GetOrderByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Status, Is.EqualTo("Paid"));
        Assert.That(result.NextStatuses, Is.EqualTo(new List<string> { "Accepted", "Cancelled" }));
        Assert.That(result.Items, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetOrderByIdAsync_NonExistingOrder_ReturnsNull()
    {
        // Act
        var result = await _service.GetOrderByIdAsync(999);

        // Assert
        Assert.That(result, Is.Null);
    }

    #endregion

    #region UpdateOrderStatusAsync Tests

    [Test]
    public async Task UpdateOrderStatusAsync_ValidTransition_UpdatesStatusAndNotifies()
    {
        // Act
        var result = await _service.UpdateOrderStatusAsync(1, OrderStatus.Accepted);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Status, Is.EqualTo("Accepted"));
        Assert.That(result.NextStatuses, Is.EqualTo(new List<string> { "Preparing", "Cancelled" }));

        // Verify notification was sent
        _notificationMock.Verify(
            x => x.NotifyOrderStatusUpdatedAsync(1, "Accepted", It.IsAny<DateTime>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateOrderStatusAsync_NonExistingOrder_ReturnsNull()
    {
        // Act
        var result = await _service.UpdateOrderStatusAsync(999, OrderStatus.Accepted);

        // Assert
        Assert.That(result, Is.Null);
        _notificationMock.Verify(
            x => x.NotifyOrderStatusUpdatedAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()),
            Times.Never);
    }

    [Test]
    public async Task UpdateOrderStatusAsync_ToTerminalState_ReturnsEmptyNextStatuses()
    {
        // Act
        var result = await _service.UpdateOrderStatusAsync(1, OrderStatus.Cancelled);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Status, Is.EqualTo("Cancelled"));
        Assert.That(result.NextStatuses, Is.Empty);

        _notificationMock.Verify(
            x => x.NotifyOrderStatusUpdatedAsync(1, "Cancelled", It.IsAny<DateTime>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateOrderStatusAsync_UpdatesTimestamp()
    {
        // Arrange
        var beforeUpdate = DateTime.UtcNow;

        // Act
        var result = await _service.UpdateOrderStatusAsync(1, OrderStatus.Accepted);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.UpdatedAt, Is.Not.Null);
        Assert.That(result.UpdatedAt, Is.GreaterThanOrEqualTo(beforeUpdate));
    }

    [Test]
    public async Task UpdateOrderStatusAsync_PersistsToDatabase()
    {
        // Act
        await _service.UpdateOrderStatusAsync(1, OrderStatus.Accepted);

        // Assert - verify the database was updated
        var order = await _dbContext.Orders.FindAsync(1);
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.Status, Is.EqualTo(OrderStatus.Accepted));
    }

    #endregion
}
