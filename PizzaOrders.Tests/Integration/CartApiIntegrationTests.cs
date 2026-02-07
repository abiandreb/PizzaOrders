using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for Cart API: create, add, get, update, remove, clear.
/// Cart is session-based (no auth required).
/// </summary>
[Category("Integration")]
public class CartApiIntegrationTests : IntegrationTestBase
{
    [SetUp]
    public async Task SetUp()
    {
        // Ensure at least one product exists for cart operations
        await EnsureProductExists();
    }

    #region Create Cart

    [Test]
    public async Task CreateCart_ReturnsNewCart()
    {
        // Act
        var response = await Client.PostAsync("/api/cart/create", null);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        Assert.That(cart, Is.Not.Null);
        Assert.That(cart!.SessionId, Is.Not.EqualTo(Guid.Empty));
    }

    #endregion

    #region Add to Cart

    [Test]
    public async Task AddToCart_ValidProduct_ReturnsOk()
    {
        // Arrange
        var sessionId = await CreateCartSession();

        // Act
        var response = await Client.PostAsJsonAsync($"/api/cart/{sessionId}/add",
            new { productId = 1, quantity = 2, toppingIds = new List<int>() });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task AddToCart_ThenGetCart_ContainsItem()
    {
        // Arrange
        var sessionId = await CreateCartSession();
        await Client.PostAsJsonAsync($"/api/cart/{sessionId}/add",
            new { productId = 1, quantity = 3, toppingIds = new List<int>() });

        // Act
        var response = await Client.GetAsync($"/api/cart/{sessionId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        Assert.That(cart, Is.Not.Null);
        Assert.That(cart!.Items, Is.Not.Null.And.Not.Empty);
    }

    #endregion

    #region Get Cart

    [Test]
    public async Task GetCart_EmptySession_ReturnsEmptyCart()
    {
        // Arrange
        var sessionId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/cart/{sessionId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var cart = await response.Content.ReadFromJsonAsync<CartDto>();
        Assert.That(cart, Is.Not.Null);
    }

    #endregion

    #region Clear Cart

    [Test]
    public async Task ClearCart_ReturnsOk()
    {
        // Arrange
        var sessionId = await CreateCartSession();
        await Client.PostAsJsonAsync($"/api/cart/{sessionId}/add",
            new { productId = 1, quantity = 1, toppingIds = new List<int>() });

        // Act
        var response = await Client.DeleteAsync($"/api/cart/{sessionId}");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Verify cart is empty
        var getResponse = await Client.GetAsync($"/api/cart/{sessionId}");
        var cart = await getResponse.Content.ReadFromJsonAsync<CartDto>();
        Assert.That(cart!.Items, Is.Null.Or.Empty);
    }

    #endregion

    #region Remove from Cart

    [Test]
    public async Task RemoveFromCart_ExistingItem_ReturnsOk()
    {
        // Arrange
        var sessionId = await CreateCartSession();
        await Client.PostAsJsonAsync($"/api/cart/{sessionId}/add",
            new { productId = 1, quantity = 1, toppingIds = new List<int>() });

        // Act
        var response = await Client.DeleteAsync($"/api/cart/{sessionId}/remove?productId=1");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    #endregion

    #region Helpers

    private async Task<Guid> CreateCartSession()
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
                    Name = "Cart Test Pizza",
                    Description = "Pizza for cart tests",
                    BasePrice = 10.00m,
                    HasToppings = true,
                    ProductType = ProductType.Pizza,
                    ImageUrl = "cart-test.jpg"
                });
#pragma warning restore CS0618
                await context.SaveChangesAsync();
            }
        });
    }

    private class CartDto
    {
        public Guid SessionId { get; set; }
        public List<CartItemDto>? Items { get; set; }
        public decimal TotalPrice { get; set; }
    }

    private class CartItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    #endregion
}
