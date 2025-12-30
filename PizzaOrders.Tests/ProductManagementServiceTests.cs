using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests
{
    [TestFixture]
    public class ProductManagementServiceTests
    {
        private AppDbContext _dbContext;
        private ProductManagementService _productManagementService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            _productManagementService = new ProductManagementService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }
        
        [Test]
        public async Task GetAllProductsAsync_WhenCalled_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<ProductEntity>
            {
                new ProductEntity { Name = "Pizza A", Description = "Desc", ImageUrl = "url", BasePrice = 10.0m },
                new ProductEntity { Name = "Pizza B", Description = "Desc", ImageUrl = "url", BasePrice = 12.0m }
            };
            await _dbContext.Products.AddRangeAsync(products);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _productManagementService.GetAllProductsAsync();

            // Assert
            var productResponses = result.ToList();
            Assert.That(productResponses, Is.Not.Null);
            Assert.That(productResponses, Is.Not.Empty);
            Assert.That(productResponses, Has.Count.EqualTo(2));
            Assert.That(productResponses[0].Name, Is.EqualTo("Pizza A"));
            Assert.That(productResponses[1].Name, Is.EqualTo("Pizza B"));
        }
    }
}
