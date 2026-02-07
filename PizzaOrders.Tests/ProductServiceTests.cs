using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class ProductServiceTests
{
    private AppDbContext _dbContext = null!;
    private Mock<ILogger<ProductService>> _loggerMock = null!;
    private ProductService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _loggerMock = new Mock<ILogger<ProductService>>();
        _service = new ProductService(_dbContext, _loggerMock.Object);

        SeedTestData();
    }

    private void SeedTestData()
    {
#pragma warning disable CS0618
        _dbContext.Products.AddRange(
            new ProductEntity { Id = 1, Name = "Margherita", Description = "Classic pizza", BasePrice = 10.00m, HasToppings = true, ProductType = ProductType.Pizza, ImageUrl = "margherita.jpg" },
            new ProductEntity { Id = 2, Name = "Pepperoni", Description = "Spicy pizza", BasePrice = 12.00m, HasToppings = true, ProductType = ProductType.Pizza, ImageUrl = "pepperoni.jpg" },
            new ProductEntity { Id = 3, Name = "Coke", Description = "Refreshing cola", BasePrice = 2.00m, HasToppings = false, ProductType = ProductType.Drink, ImageUrl = "coke.jpg" },
            new ProductEntity { Id = 4, Name = "Sprite", Description = "Lemon-lime soda", BasePrice = 2.00m, HasToppings = false, ProductType = ProductType.Drink, ImageUrl = "sprite.jpg" }
        );
#pragma warning restore CS0618
        _dbContext.SaveChanges();
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    #region GetAllProductsByType Tests

    [Test]
    public async Task GetAllProductsByType_Pizza_ReturnsOnlyPizzas()
    {
        // Act
        var result = await _service.GetAllProductsByType((int)ProductType.Pizza);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(p => p.ProductType == ProductType.Pizza), Is.True);
    }

    [Test]
    public async Task GetAllProductsByType_Drink_ReturnsOnlyDrinks()
    {
        // Act
        var result = await _service.GetAllProductsByType((int)ProductType.Drink);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(p => p.ProductType == ProductType.Drink), Is.True);
    }

    [Test]
    public async Task GetAllProductsByType_NoProductsOfType_ReturnsEmptyList()
    {
        // Act — Starter type has no products seeded
        var result = await _service.GetAllProductsByType((int)ProductType.Starter);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllProductsByType_MapsFieldsCorrectly()
    {
        // Act
        var result = await _service.GetAllProductsByType((int)ProductType.Pizza);
        var margherita = result.First(p => p.Name == "Margherita");

        // Assert
        Assert.That(margherita.Id, Is.EqualTo(1));
        Assert.That(margherita.Description, Is.EqualTo("Classic pizza"));
        Assert.That(margherita.BasePrice, Is.EqualTo(10.00m));
        Assert.That(margherita.HasToppings, Is.True);
    }

    #endregion

    #region GetProductById Tests

    [Test]
    public async Task GetProductById_ExistingProduct_ReturnsProduct()
    {
        // Act
        var result = await _service.GetProductById(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Margherita"));
        Assert.That(result.BasePrice, Is.EqualTo(10.00m));
    }

    [Test]
    public void GetProductById_NonExistingProduct_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _service.GetProductById(999));

        Assert.That(ex!.Message, Does.Contain("not found"));
    }

    [Test]
    public async Task GetProductById_MapsAllFieldsCorrectly()
    {
        // Act
        var result = await _service.GetProductById(3);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Name, Is.EqualTo("Coke"));
        Assert.That(result.Description, Is.EqualTo("Refreshing cola"));
        Assert.That(result.BasePrice, Is.EqualTo(2.00m));
        Assert.That(result.HasToppings, Is.False);
        Assert.That(result.ProductType, Is.EqualTo(ProductType.Drink));
    }

    #endregion
}
