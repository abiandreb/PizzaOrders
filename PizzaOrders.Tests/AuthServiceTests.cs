using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain.Entities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class AuthServiceTests
{
    private Mock<UserManager<ApplicationUser>> _userManagerMock;
    private Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private Mock<IConfiguration> _configurationMock;
    private AuthService _authService;
    private AppDbContext _context;

    [SetUp]
    public void Setup()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        _configurationMock = new Mock<IConfiguration>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "AuthTestDb")
            .Options;
        _context = new AppDbContext(options);

        _authService = new AuthService(_userManagerMock.Object, _roleManagerMock.Object, _context, _configurationMock.Object, Mock.Of<TokenValidationParameters>());
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Register_ShouldCreateUser()
    {
        // Arrange
        var registerDto = new RegisterDto { Email = "test@test.com", Password = "password" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(registerDto.Email)).ReturnsAsync((ApplicationUser)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerDto.Password)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());
        _configurationMock.Setup(x => x["JWT:Secret"]).Returns("YourSuperSecretKeyForTestingThatIsLongEnough");
        _configurationMock.Setup(x => x["JWT:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["JWT:Audience"]).Returns("TestAudience");

        // Act
        var result = await _authService.Register(registerDto);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Login_ShouldReturnAuthResponse_WhenCredentialsAreValid()
    {
        // Arrange
        var loginRequest = new AuthLoginRequest { Email = "test@test.com", Password = "password" };
        var user = new ApplicationUser { Email = "test@test.com", UserName = "test@test.com" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string>());
        _configurationMock.Setup(x => x["JWT:Secret"]).Returns("YourSuperSecretKeyForTestingThatIsLongEnough");
        _configurationMock.Setup(x => x["JWT:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["JWT:Audience"]).Returns("TestAudience");

        // Act
        var result = await _authService.Login(loginRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Empty);
    }
}
