using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class CheckoutServiceTests
{
    private AppDbContext _dbContext;
    private Mock<ICartService> _mockCartService;
    private Mock<ILogger<CheckoutService>> _mockLogger;
    private CheckoutService _checkoutService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _mockCartService = new Mock<ICartService>();
        _mockLogger = new Mock<ILogger<CheckoutService>>();
        _checkoutService = new CheckoutService(_mockCartService.Object, _dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    [Test]
    public async Task ProcessCheckout_EmptyCart_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _mockCartService.Setup(x => x.GetCartAsync(sessionId))
            .ReturnsAsync(new CartDto(sessionId));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _checkoutService.ProcessCheckout(sessionId));
    }

    [Test]
    public async Task ProcessCheckout_ValidCart_CreatesOrderWithRecalculatedPrices()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        var product = new ProductEntity
        {
            Id = 1,
            Name = "Pepperoni Pizza",
            Description = "Delicious pizza",
            ImageUrl = "pizza.jpg",
            BasePrice = 10.00m,
            ProductType = ProductType.Pizza
        };

        var topping = new ToppingEntity
        {
            Id = 1,
            Name = "Extra Cheese",
            Description = "Cheese topping",
            Price = 2.00m
        };

        _dbContext.Products.Add(product);
        _dbContext.Toppings.Add(topping);
        await _dbContext.SaveChangesAsync();

        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = 1,
                    Quantity = 2,
                    TotalPrice = 24.00m, // Old price from cart
                    Modifiers = new ItemModifiers
                    {
                        ExtraToppings = new List<SelectedItemTopping>
                        {
                            new SelectedItemTopping { ToppingId = 1, Price = 1.50m, Quantity = 1 } // Old topping price
                        }
                    }
                }
            }
        };

        _mockCartService.Setup(x => x.GetCartAsync(sessionId))
            .ReturnsAsync(cart);

        _mockCartService.Setup(x => x.ClearCartAsync(sessionId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.TotalPrice, Is.EqualTo(24.00m)); // (10.00 + 2.00) * 2 = 24.00 (recalculated from SQL)
        Assert.That(result.Items, Has.Count.EqualTo(1));
        Assert.That(result.Items[0].UnitPrice, Is.EqualTo(12.00m)); // 10.00 + 2.00 (recalculated)
        Assert.That(result.Status, Is.EqualTo(OrderStatus.PaymentPending.ToString()));

        // Verify order was saved
        var order = await _dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync();
        Assert.That(order, Is.Not.Null);
        Assert.That(order.TotalPrice, Is.EqualTo(24.00m));
        Assert.That(order.Items, Has.Count.EqualTo(1));

        // Verify topping price was updated to SQL price
        Assert.That(order.Items.First().ItemModifiers!.ExtraToppings[0].Price, Is.EqualTo(2.00m));

        // Verify cart was cleared
        _mockCartService.Verify(x => x.ClearCartAsync(sessionId), Times.Once);
    }

    [Test]
    public async Task ProcessCheckout_NonExistentProduct_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 999, Quantity = 1 }
            }
        };

        _mockCartService.Setup(x => x.GetCartAsync(sessionId))
            .ReturnsAsync(cart);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _checkoutService.ProcessCheckout(sessionId));
    }

    [Test]
    public async Task ProcessCheckout_MultipleItems_CalculatesTotalCorrectly()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        var product1 = new ProductEntity { Id = 1, Name = "Pizza", Description = "Pizza", ImageUrl = "pizza.jpg", BasePrice = 10.00m, ProductType = ProductType.Pizza };
        var product2 = new ProductEntity { Id = 2, Name = "Starter", Description = "Starter", ImageUrl = "starter.jpg", BasePrice = 8.00m, ProductType = ProductType.Starter };

        _dbContext.Products.AddRange(product1, product2);
        await _dbContext.SaveChangesAsync();

        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 2, TotalPrice = 20.00m },
                new CartItem { ProductId = 2, Quantity = 1, TotalPrice = 8.00m }
            }
        };

        _mockCartService.Setup(x => x.GetCartAsync(sessionId))
            .ReturnsAsync(cart);

        _mockCartService.Setup(x => x.ClearCartAsync(sessionId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId);

        // Assert
        Assert.That(result.TotalPrice, Is.EqualTo(28.00m)); // (10*2) + (8*1)
        Assert.That(result.Items, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task ProcessCheckout_WithUserId_AssignsUserToOrder()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var userId = 123;

        var product = new ProductEntity
        {
            Id = 1,
            Name = "Pizza",
            Description = "Test Pizza",
            ImageUrl = "pizza.jpg",
            BasePrice = 15.00m,
            ProductType = ProductType.Pizza
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItem>
            {
                new CartItem { ProductId = 1, Quantity = 1, TotalPrice = 15.00m }
            }
        };

        _mockCartService.Setup(x => x.GetCartAsync(sessionId))
            .ReturnsAsync(cart);

        _mockCartService.Setup(x => x.ClearCartAsync(sessionId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId, userId);

        // Assert
        Assert.That(result.OrderId, Is.GreaterThan(0));

        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == result.OrderId);
        Assert.That(order, Is.Not.Null);
        Assert.That(order.UserId, Is.EqualTo(userId));
    }

    [Test]
    public async Task ProcessCheckout_NonExistentTopping_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        var product = new ProductEntity
        {
            Id = 1,
            Name = "Pizza",
            Description = "Test Pizza",
            ImageUrl = "pizza.jpg",
            BasePrice = 10.00m,
            ProductType = ProductType.Pizza
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = 1,
                    Quantity = 1,
                    Modifiers = new ItemModifiers
                    {
                        ExtraToppings = new List<SelectedItemTopping>
                        {
                            new SelectedItemTopping { ToppingId = 999, Price = 2.00m }
                        }
                    }
                }
            }
        };

        _mockCartService.Setup(x => x.GetCartAsync(sessionId))
            .ReturnsAsync(cart);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _checkoutService.ProcessCheckout(sessionId));
    }

    [Test]
    public async Task ProcessCheckout_CartWithMultipleToppings_CalculatesCorrectly()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        var product = new ProductEntity
        {
            Id = 1,
            Name = "Pizza",
            Description = "Custom Pizza",
            ImageUrl = "pizza.jpg",
            BasePrice = 10.00m,
            ProductType = ProductType.Pizza
        };

        var topping1 = new ToppingEntity { Id = 1, Name = "Cheese", Description = "Extra cheese", Price = 2.00m };
        var topping2 = new ToppingEntity { Id = 2, Name = "Pepperoni", Description = "Pepperoni", Price = 3.00m };

        _dbContext.Products.Add(product);
        _dbContext.Toppings.AddRange(topping1, topping2);
        await _dbContext.SaveChangesAsync();

        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = 1,
                    Quantity = 1,
                    TotalPrice = 15.00m,
                    Modifiers = new ItemModifiers
                    {
                        ExtraToppings = new List<SelectedItemTopping>
                        {
                            new SelectedItemTopping { ToppingId = 1, Price = 2.00m, Quantity = 1 },
                            new SelectedItemTopping { ToppingId = 2, Price = 3.00m, Quantity = 1 }
                        }
                    }
                }
            }
        };

        _mockCartService.Setup(x => x.GetCartAsync(sessionId))
            .ReturnsAsync(cart);

        _mockCartService.Setup(x => x.ClearCartAsync(sessionId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId);

        // Assert
        Assert.That(result.TotalPrice, Is.EqualTo(15.00m)); // 10 + 2 + 3
        Assert.That(result.Items[0].UnitPrice, Is.EqualTo(15.00m));
        Assert.That(result.Items[0].Modifiers, Has.Count.EqualTo(2));
    }
}
