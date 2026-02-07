using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using NUnit.Framework;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Integration tests for Auth API: register, login, refresh token, logout.
/// </summary>
[Category("Integration")]
public class AuthApiIntegrationTests : IntegrationTestBase
{
    #region Register

    [Test]
    public async Task Register_ValidRequest_ReturnsTokens()
    {
        // Arrange
        var email = $"newuser-{Guid.NewGuid():N}@test.com";
        var request = new { email, password = "Password123!", firstName = "Test", lastName = "User" };

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/register-user", request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var result = await response.Content.ReadFromJsonAsync<AuthTokenDto>();
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Token, Is.Not.Null.And.Not.Empty);
        Assert.That(result.RefreshToken, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task Register_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange — register first
        var email = $"dup-{Guid.NewGuid():N}@test.com";
        await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });

        // Act — try again
        var response = await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Register_MissingPassword_ReturnsBadRequest()
    {
        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email = "nopass@test.com", password = "" });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    #endregion

    #region Login

    [Test]
    public async Task Login_ValidCredentials_ReturnsTokens()
    {
        // Arrange — register a user
        var email = $"login-{Guid.NewGuid():N}@test.com";
        await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/login-user",
            new { email, password = "Password123!" });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var result = await response.Content.ReadFromJsonAsync<AuthTokenDto>();
        Assert.That(result!.Token, Is.Not.Null.And.Not.Empty);
        Assert.That(result.RefreshToken, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task Login_WrongPassword_ReturnsUnauthorized()
    {
        // Arrange
        var email = $"wrongpwd-{Guid.NewGuid():N}@test.com";
        await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/login-user",
            new { email, password = "WrongPassword!" });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task Login_NonExistentUser_ReturnsUnauthorized()
    {
        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/login-user",
            new { email = "nobody@test.com", password = "Password123!" });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    #endregion

    #region Logout

    [Test]
    public async Task Logout_Authenticated_ReturnsOk()
    {
        // Arrange
        var token = await RegisterAndGetToken();
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/auth/logout");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await Client.SendAsync(request);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Logout_Unauthenticated_ReturnsUnauthorized()
    {
        // Act
        var response = await Client.PostAsync("/api/auth/logout", null);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    #endregion

    #region Refresh Token

    [Test]
    public async Task RefreshToken_ValidTokens_ReturnsNewTokens()
    {
        // Arrange — register and get tokens
        var email = $"refresh-{Guid.NewGuid():N}@test.com";
        var regResponse = await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });
        var auth = await regResponse.Content.ReadFromJsonAsync<AuthTokenDto>();

        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/refresh-token",
            new { token = auth!.Token, refreshToken = auth.RefreshToken });

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var result = await response.Content.ReadFromJsonAsync<AuthTokenDto>();
        Assert.That(result!.Token, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task RefreshToken_InvalidToken_ReturnsBadRequest()
    {
        // Act
        var response = await Client.PostAsJsonAsync("/api/auth/refresh-token",
            new { token = "invalid.jwt.token", refreshToken = "invalid-refresh" });

        // Assert
        Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.BadRequest, HttpStatusCode.Conflict));
    }

    #endregion

    #region Helpers

    private async Task<string> RegisterAndGetToken()
    {
        var email = $"helper-{Guid.NewGuid():N}@test.com";
        var response = await Client.PostAsJsonAsync("/api/auth/register-user",
            new { email, password = "Password123!" });
        response.EnsureSuccessStatusCode();
        var auth = await response.Content.ReadFromJsonAsync<AuthTokenDto>();
        return auth!.Token;
    }

    private class AuthTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    #endregion
}
