using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;

namespace PizzaOrders.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService)
    : ControllerBase
{
    [HttpPost("register-user")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await authService.Register(payload);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest payload)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            var token = await authService.Login(payload);
            return Ok(token);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest payload)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await authService.VerifyAndGenerateTokenAsync(payload);

            if (result is null) return BadRequest("Invalid tokens");

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized("Invalid user token");
        }

        await authService.LogoutAsync(userId);
        return Ok(new { message = "Logged out successfully" });
    }

    /* TODO:
        GET /confirmEmail
        POST /resendConfirmationEmail
        POST /forgotPassword
        POST /resetPassword
    */
}