
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class PizzaServiceTests
{
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _context;
    private Mock<ILogger<PizzaService>> _loggerMock;
    private PizzaService _pizzaService;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "PizzaOrdersTestDb")
            .Options;

        _context = new AppDbContext(_options);
        _loggerMock = new Mock<ILogger<PizzaService>>();
        _pizzaService = new PizzaService(_context, _loggerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetPizzasList_ShouldReturnListOfPizzas_WhenPizzasExist()
    {
        // Arrange
        await _context.Pizzas.AddRangeAsync(
            new Pizza { Name = "Pizza 1", Description = "Desc 1", Price = 10 },
            new Pizza { Name = "Pizza 2", Description = "Desc 2", Price = 12 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _pizzaService.GetPizzasList();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public void GetPizzasList_ShouldThrowInvalidOperationException_WhenNoPizzasExist()
    {
        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _pizzaService.GetPizzasList());
    }

    [Test]
    public async Task GetSinglePizza_ShouldReturnPizza_WhenPizzaExists()
    {
        // Arrange
        var pizza = new Pizza { Name = "Test Pizza", Description = "Test Desc", Price = 15 };
        await _context.Pizzas.AddAsync(pizza);
        await _context.SaveChangesAsync();

        // Act
        var result = await _pizzaService.GetSinglePizza(pizza.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Test Pizza"));
    }

    [Test]
    public void GetSinglePizza_ShouldThrowInvalidOperationException_WhenPizzaDoesNotExist()
    {
        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _pizzaService.GetSinglePizza(99));
    }

    [Test]
    public async Task AddPizza_ShouldAddPizzaToDatabase()
    {
        // Arrange
        var pizzaDto = new PizzaDto { Name = "New Pizza", Description = "New Desc", Price = 20 };

        // Act
        var result = await _pizzaService.AddPizza(pizzaDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("New Pizza"));
        Assert.That(_context.Pizzas.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task DeletePizza_ShouldRemovePizzaFromDatabase_WhenPizzaExists()
    {
        // Arrange
        var pizza = new Pizza { Name = "Test Pizza", Description = "Test Desc", Price = 15 };
        await _context.Pizzas.AddAsync(pizza);
        await _context.SaveChangesAsync();

        // Act
        await _pizzaService.DeletePizza(pizza.Id);

        // Assert
        Assert.That(_context.Pizzas.Count(), Is.EqualTo(0));
    }

    [Test]
    public void DeletePizza_ShouldThrowInvalidOperationException_WhenPizzaDoesNotExist()
    {
        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _pizzaService.DeletePizza(99));
    }
}
