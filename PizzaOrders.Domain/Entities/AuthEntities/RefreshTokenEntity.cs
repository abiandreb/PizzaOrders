using PizzaOrders.Domain.Common;

namespace PizzaOrders.Domain.Entities.AuthEntities;

public class RefreshTokenEntity : BaseEntity
{
    public string JwtId { get; set; } = null!;
    
    public string Token { get; set; } = null!;
    
    public bool IsRevoked { get; set; }
    
    public DateTime ExpiryDate { get; set; }
    
    public DateTime? DateRevoked { get; set; }
    
    public int UserId { get; set; }

    public UserEntity User { get; set; }
}
    
