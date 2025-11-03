using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> Register(RegisterDto payload);
    Task<AuthResponse> Login(AuthLoginRequest payload);
    Task<AuthResponse?> VerifyAndGenerateTokenAsync(RefreshTokenRequest payload);
}
