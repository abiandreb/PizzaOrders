namespace PizzaOrders.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string TokenHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public int? ReplacedByTokenId { get; set; }
    public string? DeviceInfo { get; set; }
    public string? Ip { get; set; }
    public string? Fingerprint { get; set; }
    public User User { get; set; }
}