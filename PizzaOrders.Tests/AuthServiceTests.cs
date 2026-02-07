using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Services;
using PizzaOrders.Domain;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Tests;

[TestFixture]
public class AuthServiceTests
{
    private Mock<IConfiguration> _configurationMock = null!;
    private Mock<UserManager<UserEntity>> _userManagerMock = null!;
    private AuthService _authService = null!;
    private AppDbContext _dbContext = null!;
    private TokenValidationParameters _tokenValidationParameters = null!;

    private const string JwtSecret = "SuperSecretTestKeyThatIsLongEnoughForHmacSha256Algorithm!";
    private const string JwtIssuer = "TestIssuer";
    private const string JwtAudience = "TestAudience";

    [SetUp]
    public void Setup()
    {
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(x => x["JWT:Secret"]).Returns(JwtSecret);
        _configurationMock.Setup(x => x["JWT:Issuer"]).Returns(JwtIssuer);
        _configurationMock.Setup(x => x["JWT:Audience"]).Returns(JwtAudience);

        var userStoreMock = new Mock<IUserStore<UserEntity>>();
        _userManagerMock = new Mock<UserManager<UserEntity>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new AppDbContext(options);

        _tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(JwtSecret)),
            ValidateIssuer = true,
            ValidIssuer = JwtIssuer,
            ValidateAudience = true,
            ValidAudience = JwtAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        _authService = new AuthService(_userManagerMock.Object, _dbContext, _configurationMock.Object, _tokenValidationParameters);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext?.Dispose();
    }

    #region Login Tests

    [Test]
    public void Login_InvalidCredentials_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var user = new UserEntity { Id = 1, Email = "test@test.com", UserName = "test@test.com" };
        var request = new AuthLoginRequest { Email = "test@test.com", Password = "WrongPassword" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(false);

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _authService.Login(request));
    }

    [Test]
    public void Login_NonExistingUser_ThrowsUnauthorizedAccessException()
    {
        // Arrange
        var request = new AuthLoginRequest { Email = "nobody@test.com", Password = "Password123!" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((UserEntity?)null);

        // Act & Assert
        Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _authService.Login(request));
    }

    [Test]
    public async Task Login_ValidCredentials_ReturnsTokens()
    {
        // Arrange
        var user = new UserEntity { Id = 1, Email = "test@test.com", UserName = "test@test.com" };
        var request = new AuthLoginRequest { Email = "test@test.com", Password = "Password123!" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _authService.Login(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
        Assert.That(result.RefreshToken, Is.Not.Null.And.Not.Empty);
        Assert.That(result.ExpiresAt, Is.Not.Null);
        Assert.That(result.ExpiresAt, Is.GreaterThan(DateTime.UtcNow));
    }

    [Test]
    public async Task Login_ValidCredentials_CreatesRefreshTokenInDatabase()
    {
        // Arrange
        var user = new UserEntity { Id = 1, Email = "test@test.com", UserName = "test@test.com" };
        var request = new AuthLoginRequest { Email = "test@test.com", Password = "Password123!" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, request.Password)).ReturnsAsync(true);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _authService.Login(request);

        // Assert
        var storedToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(t => t.Token == result.RefreshToken);
        Assert.That(storedToken, Is.Not.Null);
        Assert.That(storedToken!.UserId, Is.EqualTo(1));
        Assert.That(storedToken.IsRevoked, Is.False);
    }

    #endregion

    #region Register Tests

    [Test]
    public async Task Register_NewUser_ReturnsTokens()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Email = "new@test.com",
            Password = "Password123!",
            Role = UserRolesConstants.UserRole
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((UserEntity?)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>(), request.Password))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<UserEntity, string>((u, _) => u.Id = 5);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<UserEntity>(), UserRolesConstants.UserRole))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<UserEntity>()))
            .ReturnsAsync(new List<string> { "User" });

        // Act
        var result = await _authService.Register(request);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Token, Is.Not.Null.And.Not.Empty);
        Assert.That(result.RefreshToken, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void Register_DuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var existing = new UserEntity { Id = 1, Email = "existing@test.com", UserName = "existing@test.com" };
        var request = new RegisterUserRequest { Email = "existing@test.com", Password = "Password123!" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(existing);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _authService.Register(request));
        Assert.That(ex!.Message, Does.Contain("already exists"));
    }

    [Test]
    public void Register_WeakPassword_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RegisterUserRequest { Email = "new@test.com", Password = "123" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((UserEntity?)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>(), request.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too short" }));

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await _authService.Register(request));
        Assert.That(ex!.Message, Does.Contain("Password too short"));
    }

    [Test]
    public async Task Register_AssignsCorrectRole()
    {
        // Arrange
        var request = new RegisterUserRequest
        {
            Email = "admin@test.com",
            Password = "Password123!",
            Role = UserRolesConstants.AdminRole
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((UserEntity?)null);
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<UserEntity>(), request.Password))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<UserEntity, string>((u, _) => u.Id = 10);
        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<UserEntity>(), UserRolesConstants.AdminRole))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<UserEntity>()))
            .ReturnsAsync(new List<string> { "Admin" });

        // Act
        await _authService.Register(request);

        // Assert
        _userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<UserEntity>(), UserRolesConstants.AdminRole), Times.Once);
    }

    #endregion

    #region Logout Tests

    [Test]
    public async Task Logout_RevokesAllUserRefreshTokens()
    {
        // Arrange
        _dbContext.RefreshTokens.AddRange(
            new RefreshTokenEntity { Id = 1, UserId = 1, Token = "token1", JwtId = "jti1", IsRevoked = false, ExpiryDate = DateTime.UtcNow.AddDays(7), CreatedAt = DateTime.UtcNow },
            new RefreshTokenEntity { Id = 2, UserId = 1, Token = "token2", JwtId = "jti2", IsRevoked = false, ExpiryDate = DateTime.UtcNow.AddDays(7), CreatedAt = DateTime.UtcNow },
            new RefreshTokenEntity { Id = 3, UserId = 2, Token = "token3", JwtId = "jti3", IsRevoked = false, ExpiryDate = DateTime.UtcNow.AddDays(7), CreatedAt = DateTime.UtcNow }
        );
        await _dbContext.SaveChangesAsync();

        // Act
        await _authService.LogoutAsync(1);

        // Assert
        var user1Tokens = await _dbContext.RefreshTokens.Where(t => t.UserId == 1).ToListAsync();
        Assert.That(user1Tokens.All(t => t.IsRevoked), Is.True);

        // Other user's tokens should be untouched
        var user2Token = await _dbContext.RefreshTokens.FirstAsync(t => t.UserId == 2);
        Assert.That(user2Token.IsRevoked, Is.False);
    }

    [Test]
    public async Task Logout_NoTokens_DoesNotThrow()
    {
        // Act & Assert — should not throw
        Assert.DoesNotThrowAsync(async () => await _authService.LogoutAsync(999));
    }

    #endregion
}
