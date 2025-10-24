using System.ComponentModel.DataAnnotations;

namespace PizzaOrders.Domain.Entities;

public class RevokedAccessToken
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Jti { get; set; } = null!;
    
    public DateTime RevokedAt { get; set; }

    public string? Reason { get; set; }
    
    public DateTime ExpiresAt { get; set; }
}
