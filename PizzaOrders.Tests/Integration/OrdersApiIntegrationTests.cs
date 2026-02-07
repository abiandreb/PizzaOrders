using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using NUnit.Framework;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for the Orders API (user-facing order list and detail).
/// </summary>
[Category("Integration")]
public class OrdersApiIntegrationTests : IntegrationTestBase
{
    private string _userToken = null!;
    private int _userId;

    [SetUp]
    public async Task SetUp()
    {
        (_userToken, _userId) = await RegisterAndGetTokenWithUserId();
    }

    #region GET /api/orders

    [Test]
    public async Task GetMyOrders_Authenticated_ReturnsOk()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/orders");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetMyOrders_WithOrders_ReturnsUserOrders()
    {
        // Arrange — seed an order for this user
        await SeedOrderForUser(_userId);

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/orders");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var orders = await response.Content.ReadFromJsonAsync<List<OrderSummaryDto>>();
        Assert.That(orders, Is.Not.Null);
        Assert.That(orders!.Count, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public async Task GetMyOrders_Unauthenticated_ReturnsUnauthorized()
    {
        // Act
        var response = await Client.GetAsync("/api/orders");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    #endregion

    #region GET /api/orders/{id}

    [Test]
    public async Task GetMyOrderById_OwnOrder_ReturnsOk()
    {
        // Arrange
        var orderId = await SeedOrderForUser(_userId);
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/orders/{orderId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var order = await response.Content.ReadFromJsonAsync<OrderDetailDto>();
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.OrderId, Is.EqualTo(orderId));
    }

    [Test]
    public async Task GetMyOrderById_NonExistentOrder_ReturnsNotFound()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/orders/99999");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetMyOrderById_OtherUsersOrder_ReturnsNotFound()
    {
        // Arrange — register a second user and seed an order for them
        var otherToken = await RegisterAndGetToken("otheruser");
        var otherUserId = GetUserIdFromToken(otherToken);
        var orderId = await SeedOrderForUser(otherUserId);

        // Request using the FIRST user's token — they should not see the other user's order
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/orders/{orderId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    #endregion

    #region Helpers

    private async Task<(string token, int userId)> RegisterAndGetTokenWithUserId()
    {
        var token = await RegisterAndGetToken("orderuser");
        return (token, GetUserIdFromToken(token));
    }

    private async Task<string> RegisterAndGetToken(string prefix)
    {
        var email = $"{prefix}-{Guid.NewGuid():N}@test.com";
        var response = await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthTokenDto>();
        return auth!.Token;
    }

    private static int GetUserIdFromToken(string token)
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var userIdClaim = jwt.Claims.First(c =>
            c.Type == System.Security.Claims.ClaimTypes.NameIdentifier ||
            c.Type == "nameid" ||
            c.Type == "sub").Value;
        return int.Parse(userIdClaim);
    }

    private async Task<int> SeedOrderForUser(int userId)
    {
        var orderId = 0;
        await ExecuteDbContextAsync(async context =>
        {
            var order = new OrderEntity
            {
                UserId = userId,
                TotalPrice = 15.00m,
                Status = OrderStatus.Paid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Items = new List<OrderItemEntity>
                {
                    new()
                    {
                        ProductId = 1,
                        Quantity = 1,
                        ItemPrice = 15.00m,
                        TotalPrice = 15.00m,
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

    private class AuthTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }

    private class OrderSummaryDto
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    private class OrderDetailDto
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public int ItemCount { get; set; }
    }

    #endregion
}
