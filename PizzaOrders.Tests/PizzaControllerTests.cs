using Microsoft.AspNetCore.Mvc;
using Moq;
using PizzaOrders.API.Controllers;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.Tests;

[TestFixture]
public class PizzaControllerTests
{
    private Mock<IPizzaService> _pizzaServiceMock;
    private PizzaController _pizzaController;

    [SetUp]
    public void Setup()
    {
        _pizzaServiceMock = new Mock<IPizzaService>();
        _pizzaController = new PizzaController(_pizzaServiceMock.Object);
    }

    [Test]
    public async Task GetPizza_ShouldReturnOkResult_WhenPizzaExists()
    {
        // Arrange
        var pizzaId = 1;
        var pizzaDto = new PizzaDto { Id = pizzaId, Name = "Test Pizza" };
        _pizzaServiceMock.Setup(s => s.GetSinglePizza(pizzaId)).ReturnsAsync(pizzaDto);

        // Act
        var result = await _pizzaController.GetPizza(pizzaId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetPizza_ShouldReturnNotFoundResult_WhenPizzaDoesNotExist()
    {
        // Arrange
        var pizzaId = 1;
        _pizzaServiceMock.Setup(s => s.GetSinglePizza(pizzaId)).ReturnsAsync((PizzaDto)null);

        // Act
        var result = await _pizzaController.GetPizza(pizzaId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }
}
