using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class CartServiceTests
{
    private AppDbContext _dbContext = null!;
    private Mock<ICacheService> _cacheServiceMock = null!;
    private Mock<ILogger<CartService>> _loggerMock = null!;
    private CartService _cartService = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _cacheServiceMock = new Mock<ICacheService>();
        _loggerMock = new Mock<ILogger<CartService>>();
        _cartService = new CartService(_dbContext, _loggerMock.Object, _cacheServiceMock.Object);

        // Seed test data
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

    #region GetCartAsync Tests

    [Test]
    public async Task GetCartAsync_NewSession_ReturnsEmptyCart()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>()))
            .ReturnsAsync((CartDto?)null);

        // Act
        var result = await _cartService.GetCartAsync(sessionId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.SessionId, Is.EqualTo(sessionId));
        Assert.That(result.Items, Is.Empty);
        Assert.That(result.TotalPrice, Is.EqualTo(0));
    }

    [Test]
    public async Task GetCartAsync_ExistingSession_ReturnsCartFromCache()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var cachedCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new() { ProductId = 1, ProductName = "Margherita", Quantity = 2, TotalPrice = 20.00m }
            },
            TotalPrice = 20.00m
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>($"cart:{sessionId}"))
            .ReturnsAsync(cachedCart);

        // Act
        var result = await _cartService.GetCartAsync(sessionId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items, Has.Count.EqualTo(1));
        Assert.That(result.TotalPrice, Is.EqualTo(20.00m));
    }

    #endregion

    #region AddToCartAsync Tests

    [Test]
    public async Task AddToCartAsync_ValidProduct_AddsItemToCart()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>()))
            .ReturnsAsync((CartDto?)null);

        // Act
        await _cartService.AddToCartAsync(sessionId, productId: 1, quantity: 2, toppingIds: new List<int>());

        // Assert
        _cacheServiceMock.Verify(x => x.SetAsync(
            $"cart:{sessionId}",
            It.Is<CartDto>(c => c.Items.Count == 1 && c.Items[0].Quantity == 2),
            It.IsAny<TimeSpan?>()), Times.Once);
    }

    [Test]
    public async Task AddToCartAsync_WithToppings_CalculatesCorrectPrice()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>()))
            .ReturnsAsync((CartDto?)null);

        CartDto? capturedCart = null;
        _cacheServiceMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CartDto>(), It.IsAny<TimeSpan?>()))
            .Callback<string, CartDto, TimeSpan?>((_, cart, _) => capturedCart = cart);

        // Act - Add product ($10) with 2 toppings ($1.50 + $1.00 = $2.50), quantity 2
        await _cartService.AddToCartAsync(sessionId, productId: 1, quantity: 2, toppingIds: new List<int> { 1, 2 });

        // Assert
        Assert.That(capturedCart, Is.Not.Null);
        Assert.That(capturedCart!.Items, Has.Count.EqualTo(1));
        // Price = (10 + 1.50 + 1.00) * 2 = 25.00
        Assert.That(capturedCart.Items[0].TotalPrice, Is.EqualTo(25.00m));
        Assert.That(capturedCart.TotalPrice, Is.EqualTo(25.00m));
    }

    [Test]
    public async Task AddToCartAsync_SameProductSameToppings_IncreasesQuantity()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var existingCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    ProductName = "Margherita",
                    Quantity = 1,
                    BasePrice = 10.00m,
                    ToppingIds = new List<int> { 1 },
                    TotalPrice = 11.50m
                }
            },
            TotalPrice = 11.50m
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>($"cart:{sessionId}"))
            .ReturnsAsync(existingCart);

        CartDto? capturedCart = null;
        _cacheServiceMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CartDto>(), It.IsAny<TimeSpan?>()))
            .Callback<string, CartDto, TimeSpan?>((_, cart, _) => capturedCart = cart);

        // Act - Add same product with same toppings
        await _cartService.AddToCartAsync(sessionId, productId: 1, quantity: 2, toppingIds: new List<int> { 1 });

        // Assert
        Assert.That(capturedCart, Is.Not.Null);
        Assert.That(capturedCart!.Items, Has.Count.EqualTo(1)); // Still 1 item
        Assert.That(capturedCart.Items[0].Quantity, Is.EqualTo(3)); // 1 + 2 = 3
    }

    [Test]
    public async Task AddToCartAsync_SameProductDifferentToppings_AddsNewItem()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var existingCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    ProductName = "Margherita",
                    Quantity = 1,
                    ToppingIds = new List<int> { 1 }, // Extra Cheese
                    TotalPrice = 11.50m
                }
            },
            TotalPrice = 11.50m
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>($"cart:{sessionId}"))
            .ReturnsAsync(existingCart);

        CartDto? capturedCart = null;
        _cacheServiceMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CartDto>(), It.IsAny<TimeSpan?>()))
            .Callback<string, CartDto, TimeSpan?>((_, cart, _) => capturedCart = cart);

        // Act - Add same product with different toppings
        await _cartService.AddToCartAsync(sessionId, productId: 1, quantity: 1, toppingIds: new List<int> { 2 }); // Mushrooms

        // Assert
        Assert.That(capturedCart, Is.Not.Null);
        Assert.That(capturedCart!.Items, Has.Count.EqualTo(2)); // Now 2 items
    }

    [Test]
    public void AddToCartAsync_InvalidProduct_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>()))
            .ReturnsAsync((CartDto?)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _cartService.AddToCartAsync(sessionId, productId: 999, quantity: 1, toppingIds: new List<int>()));

        Assert.That(ex!.Message, Does.Contain("999"));
    }

    [Test]
    public void AddToCartAsync_InvalidTopping_ThrowsException()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>()))
            .ReturnsAsync((CartDto?)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _cartService.AddToCartAsync(sessionId, productId: 1, quantity: 1, toppingIds: new List<int> { 999 }));

        Assert.That(ex!.Message, Does.Contain("999"));
    }

    #endregion

    #region RemoveFromCartAsync Tests

    [Test]
    public async Task RemoveFromCartAsync_ExistingItem_RemovesFromCart()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var existingCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    ToppingIds = new List<int> { 1 },
                    TotalPrice = 11.50m
                },
                new()
                {
                    ProductId = 2,
                    ToppingIds = new List<int>(),
                    TotalPrice = 12.00m
                }
            },
            TotalPrice = 23.50m
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>($"cart:{sessionId}"))
            .ReturnsAsync(existingCart);

        CartDto? capturedCart = null;
        _cacheServiceMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CartDto>(), It.IsAny<TimeSpan?>()))
            .Callback<string, CartDto, TimeSpan?>((_, cart, _) => capturedCart = cart);

        // Act
        await _cartService.RemoveFromCartAsync(sessionId, productId: 1, toppingIds: new List<int> { 1 });

        // Assert
        Assert.That(capturedCart, Is.Not.Null);
        Assert.That(capturedCart!.Items, Has.Count.EqualTo(1));
        Assert.That(capturedCart.Items[0].ProductId, Is.EqualTo(2));
        Assert.That(capturedCart.TotalPrice, Is.EqualTo(12.00m));
    }

    [Test]
    public async Task RemoveFromCartAsync_NonExistingItem_DoesNothing()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var existingCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new() { ProductId = 1, ToppingIds = new List<int>(), TotalPrice = 10.00m }
            },
            TotalPrice = 10.00m
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>($"cart:{sessionId}"))
            .ReturnsAsync(existingCart);

        // Act
        await _cartService.RemoveFromCartAsync(sessionId, productId: 999, toppingIds: new List<int>());

        // Assert - SetAsync should not be called since item doesn't exist
        _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CartDto>(), It.IsAny<TimeSpan?>()), Times.Never);
    }

    #endregion

    #region UpdateCartAsync Tests

    [Test]
    public async Task UpdateCartAsync_ValidQuantity_UpdatesQuantity()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var existingCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 1,
                    ToppingIds = new List<int>(),
                    TotalPrice = 10.00m
                }
            },
            TotalPrice = 10.00m
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>($"cart:{sessionId}"))
            .ReturnsAsync(existingCart);

        CartDto? capturedCart = null;
        _cacheServiceMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CartDto>(), It.IsAny<TimeSpan?>()))
            .Callback<string, CartDto, TimeSpan?>((_, cart, _) => capturedCart = cart);

        // Act
        await _cartService.UpdateCartAsync(sessionId, productId: 1, quantity: 5, toppingIds: new List<int>());

        // Assert
        Assert.That(capturedCart, Is.Not.Null);
        Assert.That(capturedCart!.Items[0].Quantity, Is.EqualTo(5));
        Assert.That(capturedCart.Items[0].TotalPrice, Is.EqualTo(50.00m)); // 10 * 5
    }

    [Test]
    public async Task UpdateCartAsync_ZeroQuantity_RemovesItem()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var existingCart = new CartDto(sessionId)
        {
            Items = new List<CartItemDto>
            {
                new()
                {
                    ProductId = 1,
                    Quantity = 1,
                    ToppingIds = new List<int>(),
                    TotalPrice = 10.00m
                }
            },
            TotalPrice = 10.00m
        };

        _cacheServiceMock.Setup(x => x.GetAsync<CartDto>($"cart:{sessionId}"))
            .ReturnsAsync(existingCart);

        CartDto? capturedCart = null;
        _cacheServiceMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<CartDto>(), It.IsAny<TimeSpan?>()))
            .Callback<string, CartDto, TimeSpan?>((_, cart, _) => capturedCart = cart);

        // Act
        await _cartService.UpdateCartAsync(sessionId, productId: 1, quantity: 0, toppingIds: new List<int>());

        // Assert
        Assert.That(capturedCart, Is.Not.Null);
        Assert.That(capturedCart!.Items, Is.Empty);
        Assert.That(capturedCart.TotalPrice, Is.EqualTo(0));
    }

    #endregion

    #region ClearCartAsync Tests

    [Test]
    public async Task ClearCartAsync_RemovesCartFromCache()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        // Act
        await _cartService.ClearCartAsync(sessionId);

        // Assert
        _cacheServiceMock.Verify(x => x.RemoveAsync($"cart:{sessionId}"), Times.Once);
    }

    #endregion
}
