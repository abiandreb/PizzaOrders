using Moq;
using NUnit.Framework;
using PizzaOrders.Application.Services;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.DTOs;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PizzaOrders.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Domain.Entities.Products;
using System.Collections.Generic;
using PizzaOrders.Domain.Entities.Toppings;
using System.Linq;

namespace PizzaOrders.Tests
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<ICacheService> _cacheServiceMock;
        private Mock<ILogger<CartService>> _loggerMock;
        private AppDbContext _dbContext;
        private CartService _cartService;

        [SetUp]
        public void Setup()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _loggerMock = new Mock<ILogger<CartService>>();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use a unique name for each test
                .Options;
            _dbContext = new AppDbContext(options);
            _cartService = new CartService(_dbContext, _loggerMock.Object, _cacheServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetCartAsync_ShouldReturnNewCart_WhenCartDoesNotExist()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>()))
                .ReturnsAsync((CartDto)null);

            // Act
            var result = await _cartService.GetCartAsync(sessionId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.SessionId, Is.EqualTo(sessionId));
            Assert.That(result.Items, Is.Empty);
        }

        [Test]
        public async Task GetCartAsync_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var cart = new CartDto(sessionId);
            _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>()))
                .ReturnsAsync(cart);

            // Act
            var result = await _cartService.GetCartAsync(sessionId);

            // Assert
            Assert.That(result, Is.EqualTo(cart));
        }

        [Test]
        public async Task AddToCartAsync_ShouldAddNewItem_WhenItemDoesNotExist()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var product = new ProductEntity { Id = 1, Name = "Pizza", BasePrice = 10, Description = "Test Pizza", ImageUrl = "http://example.com/pizza.jpg" };
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var cart = new CartDto(sessionId);
            _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>())).ReturnsAsync(cart);

            // Act
            await _cartService.AddToCartAsync(sessionId, product.Id, 1, new List<int>());

            // Assert
            Assert.That(cart.Items, Has.Count.EqualTo(1));
            Assert.That(cart.Items[0].ProductId, Is.EqualTo(product.Id));
            Assert.That(cart.Items[0].Quantity, Is.EqualTo(1));
            _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), cart, null), Times.Once);
        }

        [Test]
        public async Task AddToCartAsync_ShouldUpdateQuantity_WhenItemExists()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var product = new ProductEntity { Id = 1, Name = "Pizza", BasePrice = 10, Description = "Test Pizza", ImageUrl = "http://example.com/pizza.jpg" };
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var cart = new CartDto(sessionId);
            cart.Items.Add(new CartItem { ProductId = 1, Quantity = 1 });
            _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>())).ReturnsAsync(cart);

            // Act
            await _cartService.AddToCartAsync(sessionId, product.Id, 2, new List<int>());

            // Assert
            Assert.That(cart.Items[0].Quantity, Is.EqualTo(3));
            _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), cart, null), Times.Once);
        }

        [Test]
        public async Task RemoveFromCartAsync_ShouldRemoveItem()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var cart = new CartDto(sessionId);
            cart.Items.Add(new CartItem { ProductId = 1, Quantity = 1 });
            _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>())).ReturnsAsync(cart);

            // Act
            await _cartService.RemoveFromCartAsync(sessionId, 1, new List<int>());

            // Assert
            Assert.That(cart.Items, Is.Empty);
            _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), cart, null), Times.Once);
        }

        [Test]
        public async Task ClearCartAsync_ShouldRemoveCartFromCache()
        {
            // Arrange
            var sessionId = Guid.NewGuid();

            // Act
            await _cartService.ClearCartAsync(sessionId);

            // Assert
            _cacheServiceMock.Verify(x => x.RemoveAsync($"cart:{sessionId}"), Times.Once);
        }

        [Test]
        public async Task UpdateCartAsync_ShouldUpdateItemQuantity()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var product = new ProductEntity { Id = 1, Name = "Pizza", BasePrice = 10, Description = "Test Pizza", ImageUrl = "http://example.com/pizza.jpg" };
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            var cart = new CartDto(sessionId);
            cart.Items.Add(new CartItem { ProductId = 1, Quantity = 1 });
            _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>())).ReturnsAsync(cart);

            // Act
            await _cartService.UpdateCartAsync(sessionId, 1, 5, new List<int>());

            // Assert
            Assert.That(cart.Items[0].Quantity, Is.EqualTo(5));
            _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), cart, null), Times.Once);
        }

        [Test]
        public async Task AddToCartAsync_WithToppings_ShouldCalculatePriceCorrectly()
        {
            // Arrange
            var sessionId = Guid.NewGuid();
            var product = new ProductEntity { Id = 1, Name = "Pizza", BasePrice = 10, Description = "Test Pizza", ImageUrl = "http://example.com/pizza.jpg" };
            var topping = new ToppingEntity { Id = 1, Name = "Cheese", Price = 2, Description = "Extra cheese" };
            _dbContext.Products.Add(product);
            _dbContext.Toppings.Add(topping);
            await _dbContext.SaveChangesAsync();

            var cart = new CartDto(sessionId);
            _cacheServiceMock.Setup(x => x.GetAsync<CartDto>(It.IsAny<string>())).ReturnsAsync(cart);

            // Act
            await _cartService.AddToCartAsync(sessionId, product.Id, 1, new List<int> { topping.Id });

            // Assert
            Assert.That(cart.Items, Has.Count.EqualTo(1));
            Assert.That(cart.Items[0].TotalPrice, Is.EqualTo(12));
        }
    }
}

