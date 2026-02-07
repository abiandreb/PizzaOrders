using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class ProductManagementServiceTests
{
    private AppDbContext _dbContext = null!;
    private ProductManagementService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _service = new ProductManagementService(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    #region GetAllProductsAsync Tests

    [Test]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        // Arrange
#pragma warning disable CS0618
        _dbContext.Products.AddRange(
            new ProductEntity { Name = "Pizza A", Description = "Desc A", ImageUrl = "a.jpg", BasePrice = 10.0m },
            new ProductEntity { Name = "Pizza B", Description = "Desc B", ImageUrl = "b.jpg", BasePrice = 12.0m }
        );
#pragma warning restore CS0618
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _service.GetAllProductsAsync()).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Pizza A"));
        Assert.That(result[1].Name, Is.EqualTo("Pizza B"));
    }

    [Test]
    public async Task GetAllProductsAsync_EmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var result = (await _service.GetAllProductsAsync()).ToList();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllProductsAsync_OrderedByName()
    {
        // Arrange
#pragma warning disable CS0618
        _dbContext.Products.AddRange(
            new ProductEntity { Name = "Zebra Pizza", Description = "Desc", ImageUrl = "z.jpg", BasePrice = 15.0m },
            new ProductEntity { Name = "Alpha Pizza", Description = "Desc", ImageUrl = "a.jpg", BasePrice = 10.0m }
        );
#pragma warning restore CS0618
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _service.GetAllProductsAsync()).ToList();

        // Assert
        Assert.That(result[0].Name, Is.EqualTo("Alpha Pizza"));
        Assert.That(result[1].Name, Is.EqualTo("Zebra Pizza"));
    }

    #endregion

    #region CreateProductAsync Tests

    [Test]
    public async Task CreateProductAsync_ValidRequest_ReturnsCreatedProduct()
    {
        // Arrange
        var request = new CreateProductRequestDto
        {
            Name = "New Pizza",
            Description = "Fresh pizza",
            BasePrice = 15.00m,
            HasToppings = true,
            ProductType = ProductType.Pizza,
            ImageUrl = "new.jpg"
        };

        // Act
        var result = await _service.CreateProductAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.Name, Is.EqualTo("New Pizza"));
        Assert.That(result.BasePrice, Is.EqualTo(15.00m));
        Assert.That(result.HasToppings, Is.True);
        Assert.That(result.ProductType, Is.EqualTo(ProductType.Pizza));
    }

    [Test]
    public async Task CreateProductAsync_PersistsToDatabase()
    {
        // Arrange
        var request = new CreateProductRequestDto
        {
            Name = "Pepperoni",
            Description = "Spicy pepperoni pizza",
            BasePrice = 14.00m,
            HasToppings = true,
            ProductType = ProductType.Pizza,
            ImageUrl = "pepperoni.jpg"
        };

        // Act
        var result = await _service.CreateProductAsync(request);

        // Assert
        var saved = await _dbContext.Products.FindAsync(result.Id);
        Assert.That(saved, Is.Not.Null);
        Assert.That(saved!.Name, Is.EqualTo("Pepperoni"));
        Assert.That(saved.BasePrice, Is.EqualTo(14.00m));
    }

    #endregion

    #region UpdateProductAsync Tests

    [Test]
    public async Task UpdateProductAsync_ValidRequest_UpdatesFields()
    {
        // Arrange
#pragma warning disable CS0618
        _dbContext.Products.Add(new ProductEntity { Id = 1, Name = "Old Name", Description = "Old Desc", ImageUrl = "old.jpg", BasePrice = 10.0m });
#pragma warning restore CS0618
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequestDto { Id = 1, Name = "New Name", BasePrice = 20.0m };

        // Act
        var result = await _service.UpdateProductAsync(request);

        // Assert
        Assert.That(result.Name, Is.EqualTo("New Name"));
        Assert.That(result.BasePrice, Is.EqualTo(20.0m));
        Assert.That(result.Description, Is.EqualTo("Old Desc")); // unchanged
    }

    [Test]
    public async Task UpdateProductAsync_PartialUpdate_OnlyChangesSpecifiedFields()
    {
        // Arrange
#pragma warning disable CS0618
        _dbContext.Products.Add(new ProductEntity
        {
            Id = 1, Name = "Pizza", Description = "Original desc", ImageUrl = "img.jpg",
            BasePrice = 10.0m, HasToppings = true, ProductType = ProductType.Pizza
        });
#pragma warning restore CS0618
        await _dbContext.SaveChangesAsync();

        var request = new UpdateProductRequestDto { Id = 1, Description = "Updated desc" };

        // Act
        var result = await _service.UpdateProductAsync(request);

        // Assert
        Assert.That(result.Description, Is.EqualTo("Updated desc"));
        Assert.That(result.Name, Is.EqualTo("Pizza")); // unchanged
        Assert.That(result.BasePrice, Is.EqualTo(10.0m)); // unchanged
        Assert.That(result.HasToppings, Is.True); // unchanged
    }

    [Test]
    public void UpdateProductAsync_NonExistentProduct_ThrowsException()
    {
        // Arrange
        var request = new UpdateProductRequestDto { Id = 999, Name = "Ghost" };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _service.UpdateProductAsync(request));
        Assert.That(ex!.Message, Does.Contain("not found"));
    }

    #endregion

    #region DeleteProductAsync Tests

    [Test]
    public async Task DeleteProductAsync_ExistingProduct_RemovesFromDatabase()
    {
        // Arrange
#pragma warning disable CS0618
        _dbContext.Products.Add(new ProductEntity { Id = 1, Name = "To Delete", Description = "Desc", ImageUrl = "d.jpg", BasePrice = 5.0m });
#pragma warning restore CS0618
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.DeleteProductAsync(1);

        // Assert
        var product = await _dbContext.Products.FindAsync(1);
        Assert.That(product, Is.Null);
    }

    [Test]
    public void DeleteProductAsync_NonExistentProduct_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _service.DeleteProductAsync(999));
        Assert.That(ex!.Message, Does.Contain("not found"));
    }

    #endregion
}
