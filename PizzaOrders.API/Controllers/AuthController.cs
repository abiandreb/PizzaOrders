using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities;
using PizzaOrders.Infrastructure.Data;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PizzaOrders.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    AppDbContext context,
    IConfiguration configuration,
    TokenValidationParameters tokenValidationParameters)
    : ControllerBase
{
    private readonly TokenValidationParameters _tokenValidationParameters = tokenValidationParameters;
    
    [HttpPost("register-user")]
    public async Task<IActionResult> Register([FromBody] RegisterDto payload)
    {
        var userExists = await userManager.FindByEmailAsync(payload.Email);

        if (userExists is not null)
        {
            return BadRequest("User already exists");
        }
        
        var user = new ApplicationUser
        {
            UserName = payload.Email,
            Email = payload.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        var result = await userManager.CreateAsync(user, payload.Password);
        
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        return Created(nameof(Register), user);
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest payload)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var user = await userManager.FindByEmailAsync(payload.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, payload.Password)) return Unauthorized();
        
        var token = await GenerateJwtToken(user, null);
            
        return Ok(token);

    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest payload)
    {
        try
        {
            var result = await VerifyAndGenerateTokenAsync(payload);
            
            if (result is null) return BadRequest("Invalid tokens");
            
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    private async Task<AuthResponse?> VerifyAndGenerateTokenAsync(RefreshTokenRequest payload)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var tokenVerification = jwtTokenHandler.ValidateToken(payload.Token, _tokenValidationParameters, out var validatedToken);

        if (validatedToken is JwtSecurityToken jwtSecurityToken)
        {
            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);

            if (!result) return null;
        }
        
        var utcExpiryDate = long.Parse(tokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
        var expiryDate = DateTime.UnixEpoch.AddMilliseconds(utcExpiryDate);
        
        if (expiryDate > DateTime.UtcNow)
            throw new Exception("Token is not expired");
        
        var dbRefreshToken = context.RefreshTokens.FirstOrDefault(x => x.Token == payload.RefreshToken);
        if (dbRefreshToken is null) return null;
        
        if (dbRefreshToken.IsRevoked) return null;
        
        var jti = tokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
        
        if (dbRefreshToken.JwtId != jti) return null;
        
        if (dbRefreshToken.ExpiryDate < DateTime.UtcNow) throw new Exception("Token is expired");
        
        var user = await userManager.FindByIdAsync(dbRefreshToken.UserId);
        if (user is null) return null;
        
        var token = await GenerateJwtToken(user, dbRefreshToken.Token);
        
        return token;
    }
    
    private async Task<AuthResponse> GenerateJwtToken(ApplicationUser user, string? existingRefreshToken)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            expires: DateTime.Now.AddMinutes(5),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        if (!string.IsNullOrEmpty(existingRefreshToken))
            return new AuthResponse()
            {
                Token = jwtToken,
                RefreshToken = existingRefreshToken,
                ExpiresAt = token.ValidTo
            };
        
        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsRevoked = false,
            UserId = user.Id,
            DateCreated = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            Token = Guid.NewGuid() + "-" + Guid.NewGuid()
        };

        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();

        return new AuthResponse()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = token.ValidTo
        };
    }
}