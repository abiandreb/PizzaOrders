using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IConfiguration> _configurationMock;
        private Mock<UserManager<UserEntity>> _userManagerMock;
        private Mock<RoleManager<RoleEntity>> _roleManagerMock;
        private AuthService _authService;

        private Mock<AppDbContext> _dbContextMock;
        private Mock<TokenValidationParameters> _tokenValidationParametersMock;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();

            var userStoreMock = new Mock<IUserStore<UserEntity>>();
            _userManagerMock = new Mock<UserManager<UserEntity>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContextMock = new Mock<AppDbContext>(options);

            _tokenValidationParametersMock = new Mock<TokenValidationParameters>();

            _authService = new AuthService(_userManagerMock.Object, _dbContextMock.Object, _configurationMock.Object, _tokenValidationParametersMock.Object);
        }

        [Test]
        public void LoginAsync_WithInvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var user = new UserEntity { Email = "test@test.com" };
            var loginRequest = new AuthLoginRequest { Email = "test@test.com", Password = "Password123!" };

            _userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password)).ReturnsAsync(false);
            
            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _authService.Login(loginRequest));
        }
    }
}
