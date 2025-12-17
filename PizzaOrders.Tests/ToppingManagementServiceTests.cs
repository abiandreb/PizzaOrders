using System;
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
