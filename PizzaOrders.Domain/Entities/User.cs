namespace PizzaOrders.Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; } = default!;
    
    public string Email { get; set; } = default!;
    
    public string? Phone { get; set; }
    
    public string? Address { get; set; }

    public ICollection<Order>? Orders { get; set; } = new List<Order>();
    
    public string PasswordHash { get; set; } 

    public string Roles { get; set; }

    public bool IsLocked { get; set; }
    
    public string? LockReason { get; set; }

    public DateTime? LastPasswordChangeAt { get; set; }

    public DateTime? LastLoginAt { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
    
    public ICollection<RefreshToken>? RefreshTokens { get; set; }
}