using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using NUnit.Framework;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for Checkout API: authenticated and guest checkout flows.
/// </summary>
[Category("Integration")]
public class CheckoutApiIntegrationTests : IntegrationTestBase
{
    private string _userToken = null!;

    [SetUp]
    public async Task SetUp()
    {
        _userToken = await RegisterAndGetToken();
        await EnsureProductExists();
    }

    #region Authenticated Checkout

    [Test]
    public async Task Checkout_Authenticated_WithItems_ReturnsOrder()
    {
        // Arrange — create cart and add items
        var sessionId = await CreateCartWithItem();

        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/checkout/{sessionId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var order = await response.Content.ReadFromJsonAsync<CheckoutResultDto>();
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.OrderId, Is.GreaterThan(0));
        Assert.That(order.TotalPrice, Is.GreaterThan(0));
    }

    [Test]
    public async Task Checkout_Authenticated_EmptyCart_ReturnsBadRequest()
    {
        // Arrange — create empty cart
        var sessionId = await CreateEmptyCart();

        var request = new HttpRequestMessage(HttpMethod.Post, $"/api/checkout/{sessionId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userToken);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Checkout_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        // Act — no auth header
        var response = await Client.PostAsync($"/api/checkout/{sessionId}", null);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    #endregion

    #region Guest Checkout

    [Test]
    public async Task GuestCheckout_WithItems_ReturnsOrder()
    {
        // Arrange
        var sessionId = await CreateCartWithItem();

        // Act — guest endpoint, no auth required
        var response = await Client.PostAsync($"/api/checkout/{sessionId}/guest", null);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var order = await response.Content.ReadFromJsonAsync<CheckoutResultDto>();
        Assert.That(order, Is.Not.Null);
        Assert.That(order!.OrderId, Is.GreaterThan(0));
    }

    [Test]
    public async Task GuestCheckout_EmptyCart_ReturnsBadRequest()
    {
        // Arrange
        var sessionId = await CreateEmptyCart();

        // Act
        var response = await Client.PostAsync($"/api/checkout/{sessionId}/guest", null);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    #endregion

    #region Helpers

    private async Task<string> RegisterAndGetToken()
    {
        var email = $"checkout-{Guid.NewGuid():N}@test.com";
        var response = await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthTokenDto>();
        return auth!.Token;
    }

    private async Task<Guid> CreateCartWithItem()
    {
        var createResponse = await Client.PostAsync("/api/cart/create", null);
        createResponse.EnsureSuccessStatusCode();
        var cart = await createResponse.Content.ReadFromJsonAsync<CartDto>();
        var sessionId = cart!.SessionId;

        var addResponse = await Client.PostAsJsonAsync($"/api/cart/{sessionId}/add",
            new { productId = 1, quantity = 1, toppingIds = new List<int>() });
        addResponse.EnsureSuccessStatusCode();

        return sessionId;
    }

    private async Task<Guid> CreateEmptyCart()
    {
        var response = await Client.PostAsync("/api/cart/create", null);
        response.EnsureSuccessStatusCode();
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        return cart!.SessionId;
    }

    private async Task EnsureProductExists()
    {
        await ExecuteDbContextAsync(async context =>
        {
            if (!context.Products.Any(p => p.Id == 1))
            {
#pragma warning disable CS0618
                context.Products.Add(new ProductEntity
                {
                    Name = "Checkout Test Pizza",
                    Description = "Pizza for checkout tests",
                    BasePrice = 10.00m,
                    HasToppings = false,
                    ProductType = ProductType.Pizza,
                    ImageUrl = "checkout-test.jpg"
                });
#pragma warning restore CS0618
                await context.SaveChangesAsync();
            }
        });
    }

    private class AuthTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }

    private class CartDto
    {
        public Guid SessionId { get; set; }
    }

    private class CheckoutResultDto
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    #endregion
}
