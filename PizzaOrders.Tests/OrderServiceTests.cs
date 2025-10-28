
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class OrderServiceTests
{
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _context;
    private OrderService _orderService;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "PizzaOrdersTestDb")
            .Options;

        _context = new AppDbContext(_options);
        _orderService = new OrderService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task CreateOrder_ShouldCreateOrder_WhenUserAndPizzasExist()
    {
        // Arrange
        var user = new ApplicationUser { Id = "test-user", UserName = "test" };
        await _context.Users.AddAsync(user);

        var pizzas = new[]
        {
            new Pizza { Id = 1, Name = "Pizza 1", Description = "Desc 1", Price = 10 },
            new Pizza { Id = 2, Name = "Pizza 2", Description = "Desc 2", Price = 12 }
        };
        await _context.Pizzas.AddRangeAsync(pizzas);
        await _context.SaveChangesAsync();

        var createOrderDto = new CreateOrderDto
        {
            UserId = "test-user",
            Items = new List<CreateOrderItemDto>
            {
                new() { PizzaId = 1, Quantity = 2 },
                new() { PizzaId = 2, Quantity = 1 }
            }
        };

        // Act
        var result = await _orderService.CreateOrder(createOrderDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.TotalPrice, Is.EqualTo(32)); // (10 * 2) + 12
        Assert.That(_context.Orders.Count(), Is.EqualTo(1));
    }

    [Test]
    public void CreateOrder_ShouldThrowInvalidOperationException_WhenUserDoesNotExist()
    {
        // Arrange
        var createOrderDto = new CreateOrderDto { UserId = "non-existent-user" };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.CreateOrder(createOrderDto));
    }

    [Test]
    public async Task CreateOrder_ShouldThrowInvalidOperationException_WhenPizzaDoesNotExist()
    {
        // Arrange
        var user = new ApplicationUser { Id = "test-user", UserName = "test" };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var createOrderDto = new CreateOrderDto
        {
            UserId = "test-user",
            Items = new List<CreateOrderItemDto> { new() { PizzaId = 99, Quantity = 1 } }
        };

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.CreateOrder(createOrderDto));
    }

    [Test]
    public async Task GetOrders_ShouldReturnListOfOrders()
    {
        // Arrange
        var user = new ApplicationUser { Id = "test-user", UserName = "test" };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var order = new Order
        {
            UserId = "test-user",
            Status = "New",
            TotalPrice = 22,
            CreatedAt = DateTime.UtcNow,
            OrderItems = new List<OrderItem>
            {
                new() { PizzaId = 1, Quantity = 2, ItemPrice = 11, Pizza = new Pizza { Id = 1, Name = "Pizza 1", Description = "Desc 1", Price = 11 } }
            }
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        // Act
        var result = await _orderService.GetOrders();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(1));
    }
}
