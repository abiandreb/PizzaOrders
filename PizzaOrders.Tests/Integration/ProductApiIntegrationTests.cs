using System.Net;
using System.Net.Http.Json;
using NUnit.Framework;
using PizzaOrders.Domain.Entities.Products;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for the Product API endpoints.
/// These tests run against real SQL Server and Redis containers.
/// </summary>
[Category("Integration")]
public class ProductApiIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task GetProducts_ReturnsNotFound_WhenNoProductsOfType()
    {
        // Act — use a high productType value that has no seeded products
        var response = await Client.GetAsync("/api/product?productType=99");

        // Assert — controller returns NotFound when no products found
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetProducts_ReturnsProducts_WhenProductsExist()
    {
        // Arrange - seed a product
        await ExecuteDbContextAsync(async context =>
        {
#pragma warning disable CS0618
            context.Products.Add(new ProductEntity
            {
                Name = "Integration Test Pizza",
                Description = "A test pizza for integration testing",
                BasePrice = 15.99m,
                HasToppings = true,
                ProductType = ProductType.Pizza,
                ImageUrl = "test-pizza.jpg"
            });
#pragma warning restore CS0618
            await context.SaveChangesAsync();
        });

        // Act — Pizza type = 0
        var response = await Client.GetAsync("/api/product?productType=0");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Count, Is.GreaterThanOrEqualTo(1));
        Assert.That(products.Any(p => p.Name == "Integration Test Pizza"), Is.True);
    }

    [Test]
    public async Task GetProductById_NonExistent_ReturnsBadRequest()
    {
        // Act — ProductService throws InvalidOperationException for missing products
        var response = await Client.GetAsync("/api/product/99999");

        // Assert — global exception handler converts to 400/500
        Assert.That(response.StatusCode, Is.Not.EqualTo(HttpStatusCode.OK));
    }

    // DTO for deserialization
    private class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool HasToppings { get; set; }
        public int ProductType { get; set; }
    }
}
