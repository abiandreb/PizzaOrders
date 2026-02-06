using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for the Product API endpoints.
/// These tests run against real SQL Server and Redis containers.
/// </summary>
[Category("Integration")]
public class ProductApiIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task GetProducts_ReturnsEmptyList_WhenNoProducts()
    {
        // Act
        var response = await Client.GetAsync("/api/product?productType=1");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.That(products, Is.Not.Null);
        Assert.That(products, Is.Empty);
    }

    [Test]
    public async Task GetProducts_ReturnsProducts_WhenProductsExist()
    {
        // Arrange - seed a product
        await ExecuteDbContextAsync(async context =>
        {
#pragma warning disable CS0618 // Type or member is obsolete (ImageUrl)
            context.Products.Add(new ProductEntity
            {
                Name = "Test Pizza",
                Description = "A test pizza for integration testing",
                BasePrice = 15.99m,
                HasToppings = true,
                ProductType = ProductType.Pizza,
                ImageUrl = "test-pizza.jpg"
            });
#pragma warning restore CS0618
            await context.SaveChangesAsync();
        });

        // Act
        var response = await Client.GetAsync("/api/product?productType=1"); // 1 = Pizza

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.That(products, Is.Not.Null);
        Assert.That(products!.Count, Is.GreaterThanOrEqualTo(1));
        Assert.That(products.Any(p => p.Name == "Test Pizza"), Is.True);
    }

    [Test]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Act
        var response = await Client.GetAsync("/api/product/99999");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
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
