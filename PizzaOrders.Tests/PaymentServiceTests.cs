using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Payment;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private AppDbContext _dbContext;
        private PaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            _paymentService = new PaymentService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose();
        }
        
        [Test]
        public void ProcessPayment_OrderNotFound_ThrowsException()
        {
            // Arrange
            var paymentRequest = new PaymentRequestDto { OrderId = 1, Amount = 10.0m };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.ProcessPayment(paymentRequest));
        }

        [Test]
        public async Task ProcessPayment_AmountMismatch_ThrowsException()
        {
            // Arrange
            var order = new OrderEntity { Id = 1, TotalPrice = 20.0m, Status = OrderStatus.PaymentPending };
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            var paymentRequest = new PaymentRequestDto { OrderId = 1, Amount = 10.0m };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _paymentService.ProcessPayment(paymentRequest));
        }
    }
}
