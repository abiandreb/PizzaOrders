using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class OrderServiceTests
{
    private AppDbContext _dbContext = null!;
    private Mock<ILogger<OrderService>> _loggerMock = null!;
    private OrderService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _loggerMock = new Mock<ILogger<OrderService>>();
        _service = new OrderService(_dbContext, _loggerMock.Object);

        SeedTestData();
    }

    private void SeedTestData()
    {
#pragma warning disable CS0618
        _dbContext.Products.AddRange(
            new ProductEntity { Id = 1, Name = "Margherita", Description = "Classic", BasePrice = 10.00m, HasToppings = true, ProductType = ProductType.Pizza, ImageUrl = "m.jpg" },
            new ProductEntity { Id = 2, Name = "Coke", Description = "Drink", BasePrice = 2.00m, HasToppings = false, ProductType = ProductType.Drink, ImageUrl = "c.jpg" }
        );
#pragma warning restore CS0618

        _dbContext.Toppings.Add(new ToppingEntity { Id = 1, Name = "Extra Cheese", Description = "Cheese", Price = 1.50m, Stock = 100 });

        _dbContext.Orders.AddRange(
            new OrderEntity
            {
                Id = 1, UserId = 10, TotalPrice = 23.50m, Status = OrderStatus.Paid,
                CreatedAt = DateTime.UtcNow.AddHours(-2), UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new() { Id = 1, ProductId = 1, Quantity = 2, ItemPrice = 10.00m, TotalPrice = 20.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                        ItemModifiers = new ItemModifiers { Size = "Large", ExtraToppings = new List<SelectedItemTopping> { new() { ToppingId = 1, Quantity = 1, Price = 1.50m } } } },
                    new() { Id = 2, ProductId = 2, Quantity = 1, ItemPrice = 2.00m, TotalPrice = 2.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                }
            },
            new OrderEntity
            {
                Id = 2, UserId = 10, TotalPrice = 10.00m, Status = OrderStatus.Completed,
                CreatedAt = DateTime.UtcNow.AddDays(-1), UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new() { Id = 3, ProductId = 1, Quantity = 1, ItemPrice = 10.00m, TotalPrice = 10.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
                }
            },
            new OrderEntity
            {
                Id = 3, UserId = 20, TotalPrice = 2.00m, Status = OrderStatus.Paid,
                CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new() { Id = 4, ProductId = 2, Quantity = 1, ItemPrice = 2.00m, TotalPrice = 2.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
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

    #region GetUserOrdersAsync Tests

    [Test]
    public async Task GetUserOrdersAsync_ReturnsOnlyUserOrders()
    {
        // Act
        var result = await _service.GetUserOrdersAsync(10);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(o => o.OrderId is 1 or 2), Is.True);
    }

    [Test]
    public async Task GetUserOrdersAsync_OrderedByCreatedAtDescending()
    {
        // Act
        var result = await _service.GetUserOrdersAsync(10);

        // Assert — order 1 is newer than order 2
        Assert.That(result[0].OrderId, Is.EqualTo(1));
        Assert.That(result[1].OrderId, Is.EqualTo(2));
    }

    [Test]
    public async Task GetUserOrdersAsync_NoOrders_ReturnsEmptyList()
    {
        // Act
        var result = await _service.GetUserOrdersAsync(999);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetUserOrdersAsync_IncludesItemsWithProductName()
    {
        // Act
        var result = await _service.GetUserOrdersAsync(10);

        // Assert
        var order = result.First(o => o.OrderId == 1);
        Assert.That(order.Items, Has.Count.EqualTo(2));
        Assert.That(order.Items[0].ProductName, Is.EqualTo("Margherita"));
        Assert.That(order.Items[1].ProductName, Is.EqualTo("Coke"));
    }

    [Test]
    public async Task GetUserOrdersAsync_MapsFieldsCorrectly()
    {
        // Act
        var result = await _service.GetUserOrdersAsync(10);
        var order = result.First(o => o.OrderId == 1);

        // Assert
        Assert.That(order.TotalPrice, Is.EqualTo(23.50m));
        Assert.That(order.Status, Is.EqualTo("Paid"));
        Assert.That(order.Items[0].Quantity, Is.EqualTo(2));
        Assert.That(order.Items[0].UnitPrice, Is.EqualTo(10.00m));
    }

    #endregion

    #region GetUserOrderByIdAsync Tests

    [Test]
    public async Task GetUserOrderByIdAsync_ExistingOrder_ReturnsDetail()
    {
        // Act
        var result = await _service.GetUserOrderByIdAsync(1, 10);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.OrderId, Is.EqualTo(1));
        Assert.That(result.TotalPrice, Is.EqualTo(23.50m));
        Assert.That(result.Status, Is.EqualTo("Paid"));
        Assert.That(result.ItemCount, Is.EqualTo(3)); // 2 + 1
    }

    [Test]
    public async Task GetUserOrderByIdAsync_NonExistingOrder_ReturnsNull()
    {
        // Act
        var result = await _service.GetUserOrderByIdAsync(999, 10);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUserOrderByIdAsync_WrongUser_ReturnsNull()
    {
        // Act — order 1 belongs to user 10, not user 20
        var result = await _service.GetUserOrderByIdAsync(1, 20);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetUserOrderByIdAsync_IncludesItemModifiers()
    {
        // Act
        var result = await _service.GetUserOrderByIdAsync(1, 10);

        // Assert
        Assert.That(result, Is.Not.Null);
        var pizzaItem = result!.Items.First(i => i.ProductId == 1);
        Assert.That(pizzaItem.Size, Is.EqualTo("Large"));
        Assert.That(pizzaItem.Modifiers, Has.Count.EqualTo(1));
        Assert.That(pizzaItem.Modifiers[0].ToppingName, Is.EqualTo("Extra Cheese"));
        Assert.That(pizzaItem.Modifiers[0].Price, Is.EqualTo(1.50m));
    }

    [Test]
    public async Task GetUserOrderByIdAsync_ItemWithoutModifiers_ReturnsEmptyModifiersList()
    {
        // Act
        var result = await _service.GetUserOrderByIdAsync(1, 10);

        // Assert
        var drinkItem = result!.Items.First(i => i.ProductId == 2);
        Assert.That(drinkItem.Modifiers, Is.Empty);
        Assert.That(drinkItem.Size, Is.Null);
    }

    #endregion
}
