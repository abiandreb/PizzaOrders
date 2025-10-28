namespace PizzaOrders.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string  UserId { get; set; }
    public string Token { get; set; }
    public string JwtId { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime ExpiryDate { get; set; }
    
    public ApplicationUser User { get; set; }
}