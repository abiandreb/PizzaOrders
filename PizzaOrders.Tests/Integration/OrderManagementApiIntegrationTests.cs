using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for Order Management API including SignalR real-time notifications
/// and workflow next-status transitions.
/// </summary>
[Category("Integration")]
public class OrderManagementApiIntegrationTests : IntegrationTestBase
{
    private string _adminToken = null!;
    private string _userToken = null!;

    [SetUp]
    public async Task SetUp()
    {
        _adminToken = await LoginAsAdmin();
        _userToken = await LoginAsUser();
    }

    #region NextStatuses in API Response

    [Test]
    public async Task GetAllOrders_ReturnsNextStatuses_ForEachOrder()
    {
        // Arrange - seed an order with Paid status
        var orderId = await SeedOrderAsync(OrderStatus.Paid);

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/management/orders");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var orders = await response.Content.ReadFromJsonAsync<List<OrderAdminResponseDto>>();
        Assert.That(orders, Is.Not.Null);

        var order = orders!.FirstOrDefault(o => o.Id == orderId);
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.NextStatuses, Is.Not.Null);
        Assert.That(order.NextStatuses, Does.Contain("Accepted"));
        Assert.That(order.NextStatuses, Does.Contain("Cancelled"));
    }

    [Test]
    public async Task GetOrderById_ReturnsNextStatuses()
    {
        // Arrange
        var orderId = await SeedOrderAsync(OrderStatus.Preparing);

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/management/orders/{orderId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var order = await response.Content.ReadFromJsonAsync<OrderAdminResponseDto>();
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.NextStatuses, Does.Contain("Ready"));
        Assert.That(order.NextStatuses, Does.Contain("Cancelled"));
    }

    [Test]
    public async Task GetOrderById_TerminalStatus_ReturnsEmptyNextStatuses()
    {
        // Arrange
        var orderId = await SeedOrderAsync(OrderStatus.Completed);

        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/management/orders/{orderId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var order = await response.Content.ReadFromJsonAsync<OrderAdminResponseDto>();
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.NextStatuses, Is.Empty);
    }

    #endregion

    #region UpdateOrderStatus with NextStatuses

    [Test]
    public async Task UpdateOrderStatus_ReturnsUpdatedNextStatuses()
    {
        // Arrange - create a Paid order
        var orderId = await SeedOrderAsync(OrderStatus.Paid);

        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/management/orders/{orderId}/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        request.Content = JsonContent.Create(new { status = "Accepted" });

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var order = await response.Content.ReadFromJsonAsync<OrderAdminResponseDto>();
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.Status, Is.EqualTo("Accepted"));
        Assert.That(order.NextStatuses, Does.Contain("Preparing"));
        Assert.That(order.NextStatuses, Does.Contain("Cancelled"));
    }

    [Test]
    public async Task UpdateOrderStatus_ToCancelled_ReturnsEmptyNextStatuses()
    {
        // Arrange
        var orderId = await SeedOrderAsync(OrderStatus.Paid);

        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/management/orders/{orderId}/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        request.Content = JsonContent.Create(new { status = "Cancelled" });

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var order = await response.Content.ReadFromJsonAsync<OrderAdminResponseDto>();
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.Status, Is.EqualTo("Cancelled"));
        Assert.That(order.NextStatuses, Is.Empty);
    }

    [Test]
    public async Task UpdateOrderStatus_NonExistingOrder_ReturnsNotFound()
    {
        var request = new HttpRequestMessage(HttpMethod.Put, "/api/management/orders/99999/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        request.Content = JsonContent.Create(new { status = "Accepted" });

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateOrderStatus_InvalidStatus_ReturnsBadRequest()
    {
        var orderId = await SeedOrderAsync(OrderStatus.Paid);

        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/management/orders/{orderId}/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        request.Content = JsonContent.Create(new { status = "NotARealStatus" });

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    #endregion

    #region SignalR Real-Time Notifications

    [Test]
    public async Task SignalR_ReceivesStatusUpdate_WhenOrderStatusChanges()
    {
        // Arrange - create an order
        var orderId = await SeedOrderAsync(OrderStatus.Paid);

        // Connect to SignalR hub as the user
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{Client.BaseAddress}hubs/orders", options =>
            {
                options.HttpMessageHandlerFactory = _ => Factory.Server.CreateHandler();
                options.AccessTokenProvider = () => Task.FromResult<string?>(_userToken);
            })
            .Build();

        string? receivedStatus = null;
        int? receivedOrderId = null;
        var statusReceived = new TaskCompletionSource<bool>();

        hubConnection.On<object>("OrderStatusUpdated", (data) =>
        {
            // Parse the anonymous object via JSON
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            var doc = System.Text.Json.JsonDocument.Parse(json);
            receivedOrderId = doc.RootElement.GetProperty("orderId").GetInt32();
            receivedStatus = doc.RootElement.GetProperty("status").GetString();
            statusReceived.TrySetResult(true);
        });

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("JoinOrderGroup", orderId);

        // Act - admin updates the order status
        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/management/orders/{orderId}/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        request.Content = JsonContent.Create(new { status = "Accepted" });
        await Client.SendAsync(request);

        // Assert - wait for SignalR notification (with timeout)
        var completed = await Task.WhenAny(statusReceived.Task, Task.Delay(TimeSpan.FromSeconds(10)));
        Assert.That(completed, Is.EqualTo(statusReceived.Task), "SignalR notification was not received within timeout");
        Assert.That(receivedOrderId, Is.EqualTo(orderId));
        Assert.That(receivedStatus, Is.EqualTo("Accepted"));

        await hubConnection.DisposeAsync();
    }

    [Test]
    public async Task SignalR_DoesNotReceiveUpdate_ForDifferentOrder()
    {
        // Arrange - create two orders
        var orderId1 = await SeedOrderAsync(OrderStatus.Paid);
        var orderId2 = await SeedOrderAsync(OrderStatus.Paid);

        // Connect to SignalR hub and join only order 1's group
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{Client.BaseAddress}hubs/orders", options =>
            {
                options.HttpMessageHandlerFactory = _ => Factory.Server.CreateHandler();
                options.AccessTokenProvider = () => Task.FromResult<string?>(_userToken);
            })
            .Build();

        var receivedUpdates = new List<int>();
        hubConnection.On<object>("OrderStatusUpdated", (data) =>
        {
            var json = System.Text.Json.JsonSerializer.Serialize(data);
            var doc = System.Text.Json.JsonDocument.Parse(json);
            receivedUpdates.Add(doc.RootElement.GetProperty("orderId").GetInt32());
        });

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("JoinOrderGroup", orderId1);

        // Act - admin updates order 2 (not the one we're listening to)
        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/management/orders/{orderId2}/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        request.Content = JsonContent.Create(new { status = "Accepted" });
        await Client.SendAsync(request);

        // Wait briefly to ensure no message arrives
        await Task.Delay(TimeSpan.FromSeconds(2));

        // Assert
        Assert.That(receivedUpdates, Does.Not.Contain(orderId2));

        await hubConnection.DisposeAsync();
    }

    [Test]
    public async Task SignalR_RequiresAuthentication()
    {
        // Arrange - try to connect without a token
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{Client.BaseAddress}hubs/orders", options =>
            {
                options.HttpMessageHandlerFactory = _ => Factory.Server.CreateHandler();
                // No access token
            })
            .Build();

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(async () => await hubConnection.StartAsync());

        await hubConnection.DisposeAsync();
    }

    #endregion

    #region Authorization

    [Test]
    public async Task GetAllOrders_AsRegularUser_ReturnsForbidden()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/management/orders");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    [Test]
    public async Task UpdateOrderStatus_AsRegularUser_ReturnsForbidden()
    {
        var orderId = await SeedOrderAsync(OrderStatus.Paid);

        var request = new HttpRequestMessage(HttpMethod.Put, $"/api/management/orders/{orderId}/status");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);
        request.Content = JsonContent.Create(new { status = "Accepted" });

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
    }

    #endregion

    #region Helpers

    private async Task<string> LoginAsAdmin()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login-user", new
        {
            email = "admin@pizzaorders.com",
            password = "Admin123!"
        });

        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return auth!.Token;
    }

    private async Task<string> LoginAsUser()
    {
        var response = await Client.PostAsJsonAsync("/api/auth/login-user", new
        {
            email = "user@pizzaorders.com",
            password = "User123!"
        });

        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        return auth!.Token;
    }

    private async Task<int> SeedOrderAsync(OrderStatus status)
    {
        int orderId = 0;
        await ExecuteDbContextAsync(async context =>
        {
            // Product seeded by migrations, no need to check

            var order = new OrderEntity
            {
                UserId = 2, // Regular user
                TotalPrice = 10.00m,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new()
                    {
                        ProductId = 1,
                        Quantity = 1,
                        ItemPrice = 10.00m,
                        TotalPrice = 10.00m,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                }
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            orderId = order.Id;
        });
        return orderId;
    }

    // DTOs for deserialization
    private class OrderAdminResponseDto
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? UserEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> NextStatuses { get; set; } = new();
    }

    private class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }

    #endregion
}
