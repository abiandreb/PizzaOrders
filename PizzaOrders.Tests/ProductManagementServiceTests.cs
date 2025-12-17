using System;
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
    }
}
