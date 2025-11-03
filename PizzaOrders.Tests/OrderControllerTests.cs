using Microsoft.AspNetCore.Mvc;
using Moq;
using PizzaOrders.API.Controllers;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.Tests;

[TestFixture]
public class OrderControllerTests
{
    private Mock<IOrderService> _orderServiceMock;
    private OrderController _orderController;

    [SetUp]
    public void Setup()
    {
        _orderServiceMock = new Mock<IOrderService>();
        _orderController = new OrderController(_orderServiceMock.Object);
    }

    [Test]
    public async Task GetOrdersByUserId_ShouldReturnOkResult()
    {
        // Arrange
        var userId = "test-user";
        _orderServiceMock.Setup(s => s.GetOrdersByUserId(userId)).ReturnsAsync(new List<OrderObject>());

        // Act
        var result = await _orderController.GetOrdersByUserId(userId);

        // Assert
        Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<OrderObject>>>());
    }

    [Test]
    public async Task UpdateOrderStatus_ShouldReturnNoContentResult()
    {
        // Arrange
        var orderId = "test-order";
        var status = "Completed";
        _orderServiceMock.Setup(s => s.UpdateOrderStatus(orderId, status)).Returns(Task.CompletedTask);

        // Act
        var result = await _orderController.UpdateOrderStatus(orderId, status);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}
