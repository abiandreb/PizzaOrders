using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class ToppingManagementServiceTests
{
    private AppDbContext _dbContext = null!;
    private ToppingManagementService _service = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _service = new ToppingManagementService(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    #region GetAllToppingsAsync Tests

    [Test]
    public async Task GetAllToppingsAsync_ReturnsAllToppings()
    {
        // Arrange
        _dbContext.Toppings.AddRange(
            new ToppingEntity { Name = "Cheese", Description = "Extra cheese", Price = 1.50m, Stock = 100 },
            new ToppingEntity { Name = "Mushrooms", Description = "Fresh mushrooms", Price = 1.00m, Stock = 50 }
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _service.GetAllToppingsAsync()).ToList();

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task GetAllToppingsAsync_OrderedByName()
    {
        // Arrange
        _dbContext.Toppings.AddRange(
            new ToppingEntity { Name = "Zucchini", Description = "Desc", Price = 1.00m, Stock = 50 },
            new ToppingEntity { Name = "Artichoke", Description = "Desc", Price = 1.50m, Stock = 30 }
        );
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _service.GetAllToppingsAsync()).ToList();

        // Assert
        Assert.That(result[0].Name, Is.EqualTo("Artichoke"));
        Assert.That(result[1].Name, Is.EqualTo("Zucchini"));
    }

    [Test]
    public async Task GetAllToppingsAsync_EmptyDatabase_ReturnsEmptyList()
    {
        // Act
        var result = (await _service.GetAllToppingsAsync()).ToList();

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetAllToppingsAsync_MapsFieldsCorrectly()
    {
        // Arrange
        _dbContext.Toppings.Add(new ToppingEntity { Name = "Olives", Description = "Black olives", Price = 0.75m, Stock = 200 });
        await _dbContext.SaveChangesAsync();

        // Act
        var result = (await _service.GetAllToppingsAsync()).ToList();
        var topping = result.Single();

        // Assert
        Assert.That(topping.Id, Is.GreaterThan(0));
        Assert.That(topping.Name, Is.EqualTo("Olives"));
        Assert.That(topping.Description, Is.EqualTo("Black olives"));
        Assert.That(topping.Price, Is.EqualTo(0.75m));
        Assert.That(topping.Stock, Is.EqualTo(200));
    }

    #endregion

    #region CreateToppingAsync Tests

    [Test]
    public async Task CreateToppingAsync_ValidRequest_ReturnsToppingResponse()
    {
        // Arrange
        var request = new CreateToppingRequestDto
        {
            Name = "Pepperoni",
            Description = "Spicy pepperoni slices",
            Price = 2.00m,
            Stock = 150
        };

        // Act
        var result = await _service.CreateToppingAsync(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.Name, Is.EqualTo("Pepperoni"));
        Assert.That(result.Price, Is.EqualTo(2.00m));
        Assert.That(result.Stock, Is.EqualTo(150));
    }

    [Test]
    public async Task CreateToppingAsync_PersistsToDatabase()
    {
        // Arrange
        var request = new CreateToppingRequestDto
        {
            Name = "Bacon",
            Description = "Crispy bacon bits",
            Price = 2.50m,
            Stock = 80
        };

        // Act
        var result = await _service.CreateToppingAsync(request);

        // Assert
        var saved = await _dbContext.Toppings.FindAsync(result.Id);
        Assert.That(saved, Is.Not.Null);
        Assert.That(saved!.Name, Is.EqualTo("Bacon"));
        Assert.That(saved.Price, Is.EqualTo(2.50m));
    }

    #endregion

    #region UpdateToppingAsync Tests

    [Test]
    public async Task UpdateToppingAsync_ValidRequest_UpdatesFields()
    {
        // Arrange
        _dbContext.Toppings.Add(new ToppingEntity { Id = 1, Name = "Old Name", Description = "Old Desc", Price = 1.00m, Stock = 50 });
        await _dbContext.SaveChangesAsync();

        var request = new UpdateToppingRequestDto { Id = 1, Name = "New Name", Price = 2.00m };

        // Act
        var result = await _service.UpdateToppingAsync(request);

        // Assert
        Assert.That(result.Name, Is.EqualTo("New Name"));
        Assert.That(result.Price, Is.EqualTo(2.00m));
        Assert.That(result.Description, Is.EqualTo("Old Desc")); // unchanged
        Assert.That(result.Stock, Is.EqualTo(50)); // unchanged
    }

    [Test]
    public async Task UpdateToppingAsync_PartialUpdate_OnlyChangesSpecifiedFields()
    {
        // Arrange
        _dbContext.Toppings.Add(new ToppingEntity { Id = 1, Name = "Cheese", Description = "Extra cheese", Price = 1.50m, Stock = 100 });
        await _dbContext.SaveChangesAsync();

        var request = new UpdateToppingRequestDto { Id = 1, Stock = 200 };

        // Act
        var result = await _service.UpdateToppingAsync(request);

        // Assert
        Assert.That(result.Stock, Is.EqualTo(200));
        Assert.That(result.Name, Is.EqualTo("Cheese")); // unchanged
        Assert.That(result.Price, Is.EqualTo(1.50m)); // unchanged
    }

    [Test]
    public void UpdateToppingAsync_NonExistentTopping_ThrowsException()
    {
        // Arrange
        var request = new UpdateToppingRequestDto { Id = 999, Name = "Ghost" };

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _service.UpdateToppingAsync(request));
        Assert.That(ex!.Message, Does.Contain("not found"));
    }

    #endregion

    #region DeleteToppingAsync Tests

    [Test]
    public async Task DeleteToppingAsync_ExistingTopping_RemovesFromDatabase()
    {
        // Arrange
        _dbContext.Toppings.Add(new ToppingEntity { Id = 1, Name = "To Delete", Description = "Desc", Price = 1.00m, Stock = 10 });
        await _dbContext.SaveChangesAsync();

        // Act
        await _service.DeleteToppingAsync(1);

        // Assert
        var topping = await _dbContext.Toppings.FindAsync(1);
        Assert.That(topping, Is.Null);
    }

    [Test]
    public void DeleteToppingAsync_NonExistentTopping_ThrowsException()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _service.DeleteToppingAsync(999));
        Assert.That(ex!.Message, Does.Contain("not found"));
    }

    #endregion
}
