using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Infrastructure.Data;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PizzaOrders.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AuthService(
        UserManager<UserEntity> userManager,
        AppDbContext context,
        IConfiguration configuration,
        TokenValidationParameters tokenValidationParameters)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
        _tokenValidationParameters = tokenValidationParameters;
    }

    public async Task<AuthResponse> Register(RegisterUserRequest payload)
    {
        var userExists = await _userManager.FindByEmailAsync(payload.Email);

        if (userExists is not null)
        {
            throw new InvalidOperationException("User already exists");
        }
        
        var user = new UserEntity
        {
            UserName = payload.Email,
            Email = payload.Email,
            EmailConfirmed = true,
            PhoneNumber = payload.PhoneNumber,
            Address = payload.Address
        };
        
        var result = await _userManager.CreateAsync(user, payload.Password);
        
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        
        await _userManager.AddToRoleAsync(user, payload.Role);
        
        return await GenerateJwtToken(user, null);
    }

    public async Task<AuthResponse> Login(AuthLoginRequest payload)
    {
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, payload.Password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }
        
        return await GenerateJwtToken(user, null);
    }

    public async Task<AuthResponse?> VerifyAndGenerateTokenAsync(RefreshTokenRequest payload)
    {
        try
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var tokenVerification =
                jwtTokenHandler.ValidateToken(payload.Token, _tokenValidationParameters, out var validatedToken);

            if (validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);

                if (!result) return null;
            }

            var utcExpiryDate =
                long.Parse(tokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var expiryDate = DateTime.UnixEpoch.AddMilliseconds(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
                throw new InvalidOperationException("Token is not expired");

            var dbRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == payload.RefreshToken);
            if (dbRefreshToken is null) return null;

            if (dbRefreshToken.IsRevoked) return null;

            var jti = tokenVerification.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            if (dbRefreshToken.JwtId != jti) return null;

            if (dbRefreshToken.ExpiryDate < DateTime.UtcNow) throw new InvalidOperationException("Token is expired");

            var user = await _userManager.FindByIdAsync(dbRefreshToken.UserId.ToString());
            if (user is null) return null;

            var token = await GenerateJwtToken(user, dbRefreshToken.Token);

            return token;
        }
        catch (SecurityTokenExpiredException)
        {
            var dbRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == payload.RefreshToken);

            var user = await _userManager.FindByIdAsync(dbRefreshToken?.UserId.ToString() ?? string.Empty);
            if (user is null) return null;

            var token = await GenerateJwtToken(user, dbRefreshToken?.Token);

            return token;
        }
    }
    
    private async Task<AuthResponse> GenerateJwtToken(UserEntity user, string? existingRefreshToken)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
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
        
        var refreshToken = new RefreshTokenEntity()
        {
            JwtId = token.Id,
            IsRevoked = false,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            Token = Guid.NewGuid() + "-" + Guid.NewGuid()
        };

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return new AuthResponse()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            ExpiresAt = token.ValidTo
        };
    }
}