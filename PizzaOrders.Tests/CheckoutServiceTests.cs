using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
    private AppDbContext _dbContext = null!;
    private Mock<ICartService> _cartServiceMock = null!;
    private Mock<ILogger<CheckoutService>> _loggerMock = null!;
    private CheckoutService _checkoutService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new AppDbContext(options);
        _cartServiceMock = new Mock<ICartService>();
        _loggerMock = new Mock<ILogger<CheckoutService>>();
        _checkoutService = new CheckoutService(_cartServiceMock.Object, _dbContext, _loggerMock.Object);

        SeedTestData();
    }

    private void SeedTestData()
    {
#pragma warning disable CS0618 // Type or member is obsolete (ImageUrl)
        _dbContext.Products.AddRange(
            new ProductEntity
            {
                Id = 1,
                Name = "Margherita",
                Description = "Classic pizza with tomato sauce and mozzarella",
                BasePrice = 10.00m,
                HasToppings = true,
                ProductType = ProductType.Pizza,
                ImageUrl = "margherita.jpg"
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Pepperoni",
                Description = "Pizza with pepperoni and mozzarella",
                BasePrice = 12.00m,
                HasToppings = true,
                ProductType = ProductType.Pizza,
                ImageUrl = "pepperoni.jpg"
            },
            new ProductEntity
            {
                Id = 3,
                Name = "Coke",
                Description = "Refreshing cola drink",
                BasePrice = 2.00m,
                HasToppings = false,
                ProductType = ProductType.Drink,
                ImageUrl = "coke.jpg"
            }
        );
#pragma warning restore CS0618

        _dbContext.Toppings.AddRange(
            new ToppingEntity { Id = 1, Name = "Extra Cheese", Description = "Extra mozzarella cheese", Price = 1.50m, Stock = 100 },
            new ToppingEntity { Id = 2, Name = "Mushrooms", Description = "Fresh sliced mushrooms", Price = 1.00m, Stock = 100 },
            new ToppingEntity { Id = 3, Name = "Pepperoni", Description = "Spicy pepperoni slices", Price = 2.00m, Stock = 100 }
        );

        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    #region ProcessCheckout Tests

    [Test]
    public async Task ProcessCheckout_ValidCart_CreatesOrder()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    ProductName = "Margherita",
                    Quantity = 2,
                    BasePrice = 10.00m,
                    ToppingIds = new List<int>(),
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 20.00m
                }
            },
            TotalPrice = 20.00m
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);
        _cartServiceMock.Setup(x => x.ClearCartAsync(sessionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId, userId: null);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.TotalPrice, Is.EqualTo(20.00m));
        Assert.That(result.Status, Is.EqualTo(OrderStatus.PaymentPending.ToString()));
        Assert.That(result.Items, Has.Count.EqualTo(1));

        // Verify order was saved
        var savedOrder = await _dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync();
        Assert.That(savedOrder, Is.Not.Null);
        Assert.That(savedOrder!.TotalPrice, Is.EqualTo(20.00m));
    }

    [Test]
    public async Task ProcessCheckout_WithToppings_CalculatesCorrectTotal()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    ProductName = "Margherita",
                    Quantity = 1,
                    BasePrice = 10.00m,
                    ToppingIds = new List<int> { 1, 2 }, // Extra Cheese ($1.50) + Mushrooms ($1.00)
                    Toppings = new List<CartToppingDto>
                    {
                        new() { ToppingId = 1, ToppingName = "Extra Cheese", Price = 1.50m },
                        new() { ToppingId = 2, ToppingName = "Mushrooms", Price = 1.00m }
                    },
                    TotalPrice = 12.50m
                }
            },
            TotalPrice = 12.50m
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);
        _cartServiceMock.Setup(x => x.ClearCartAsync(sessionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId, userId: null);

        // Assert
        Assert.That(result.TotalPrice, Is.EqualTo(12.50m));
        Assert.That(result.Items[0].Modifiers, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task ProcessCheckout_WithUserId_AssignsUserToOrder()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var userId = 42;
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 3,
                    ProductName = "Coke",
                    Quantity = 1,
                    ToppingIds = new List<int>(),
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 2.00m
                }
            },
            TotalPrice = 2.00m
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);
        _cartServiceMock.Setup(x => x.ClearCartAsync(sessionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId, userId: userId);

        // Assert
        var savedOrder = await _dbContext.Orders.FirstOrDefaultAsync();
        Assert.That(savedOrder, Is.Not.Null);
        Assert.That(savedOrder!.UserId, Is.EqualTo(userId));
    }

    [Test]
    public async Task ProcessCheckout_MultipleItems_CalculatesCorrectTotal()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    ProductName = "Margherita",
                    Quantity = 2,
                    ToppingIds = new List<int>(),
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 20.00m // 10 * 2
                },
                new()
                {
                    ProductId = 2,
                    ProductName = "Pepperoni",
                    Quantity = 1,
                    ToppingIds = new List<int> { 3 }, // Extra Pepperoni ($2.00)
                    Toppings = new List<CartToppingDto>
                    {
                        new() { ToppingId = 3, ToppingName = "Pepperoni", Price = 2.00m }
                    },
                    TotalPrice = 14.00m // 12 + 2
                },
                new()
                {
                    ProductId = 3,
                    ProductName = "Coke",
                    Quantity = 3,
                    ToppingIds = new List<int>(),
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 6.00m // 2 * 3
                }
            },
            TotalPrice = 40.00m // 20 + 14 + 6
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);
        _cartServiceMock.Setup(x => x.ClearCartAsync(sessionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId, userId: null);

        // Assert
        Assert.That(result.TotalPrice, Is.EqualTo(40.00m));
        Assert.That(result.Items, Has.Count.EqualTo(3));
    }

    [Test]
    public void ProcessCheckout_EmptyCart_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var emptyCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>(),
            TotalPrice = 0
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(emptyCart);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _checkoutService.ProcessCheckout(sessionId));

        Assert.That(ex!.Message, Does.Contain("empty"));
    }

    [Test]
    public void ProcessCheckout_NullCart_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var nullItemsCart = new CartDto(sessionId) { Items = null! };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(nullItemsCart);

        // Act & Assert - throws ArgumentNullException from LINQ when Items is null
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _checkoutService.ProcessCheckout(sessionId));
    }

    [Test]
    public void ProcessCheckout_InvalidProduct_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 999, // Non-existent product
                    Quantity = 1,
                    ToppingIds = new List<int>(),
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 10.00m
                }
            },
            TotalPrice = 10.00m
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _checkoutService.ProcessCheckout(sessionId));

        Assert.That(ex!.Message, Does.Contain("999"));
    }

    [Test]
    public void ProcessCheckout_InvalidTopping_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 1,
                    ToppingIds = new List<int> { 999 }, // Non-existent topping
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 10.00m
                }
            },
            TotalPrice = 10.00m
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _checkoutService.ProcessCheckout(sessionId));

        Assert.That(ex!.Message, Does.Contain("999"));
    }

    [Test]
    public async Task ProcessCheckout_ClearsCartAfterSuccess()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 1,
                    ToppingIds = new List<int>(),
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 10.00m
                }
            },
            TotalPrice = 10.00m
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);
        _cartServiceMock.Setup(x => x.ClearCartAsync(sessionId)).Returns(Task.CompletedTask);

        // Act
        await _checkoutService.ProcessCheckout(sessionId);

        // Assert
        _cartServiceMock.Verify(x => x.ClearCartAsync(sessionId), Times.Once);
    }

    [Test]
    public async Task ProcessCheckout_SetsCorrectOrderStatus()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 1,
                    ToppingIds = new List<int>(),
                    Toppings = new List<CartToppingDto>(),
                    TotalPrice = 10.00m
                }
            },
            TotalPrice = 10.00m
        };

        _cartServiceMock.Setup(x => x.GetCartAsync(sessionId)).ReturnsAsync(cart);
        _cartServiceMock.Setup(x => x.ClearCartAsync(sessionId)).Returns(Task.CompletedTask);

        // Act
        var result = await _checkoutService.ProcessCheckout(sessionId);

        // Assert
        Assert.That(result.Status, Is.EqualTo(OrderStatus.PaymentPending.ToString()));

        var savedOrder = await _dbContext.Orders.FirstOrDefaultAsync();
        Assert.That(savedOrder!.Status, Is.EqualTo(OrderStatus.PaymentPending));
    }

    #endregion
}
