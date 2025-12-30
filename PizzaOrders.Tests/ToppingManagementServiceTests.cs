using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests
{
    [TestFixture]
    public class ToppingManagementServiceTests
    {
        private AppDbContext _dbContext;
        private ToppingManagementService _toppingManagementService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            _toppingManagementService = new ToppingManagementService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }

        [Test]
        public async Task GetAllToppingsAsync_WhenCalled_ReturnsAllToppings()
        {
            // Arrange
            var toppings = new List<ToppingEntity>
            {
                new ToppingEntity { Name = "Topping A", Description = "Desc", Price = 1.0m },
                new ToppingEntity { Name = "Topping B", Description = "Desc", Price = 1.5m }
            };
            await _dbContext.Toppings.AddRangeAsync(toppings);
            await _dbContext.SaveChangesAsync();
            
            // Act
            var result = await _toppingManagementService.GetAllToppingsAsync();
            
            // Assert
            var toppingResponseDtos = result.ToList();
            Assert.That(toppingResponseDtos, Is.Not.Null);
            Assert.That(toppingResponseDtos, Is.Not.Empty);
            Assert.That(toppingResponseDtos, Has.Count.EqualTo(2));
            Assert.That(toppingResponseDtos[0].Name, Is.EqualTo("Topping A"));
        }

        [Test]
        public void UpdateToppingAsync_NonExistentTopping_ThrowsException()
        {
            // Arrange
            var request = new UpdateToppingRequestDto { Id = 999, Name = "New Name" };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _toppingManagementService.UpdateToppingAsync(request));
        }
        
        [Test]
        public void DeleteToppingAsync_NonExistentTopping_ThrowsException()
        {
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _toppingManagementService.DeleteToppingAsync(999));
        }
    }
}
