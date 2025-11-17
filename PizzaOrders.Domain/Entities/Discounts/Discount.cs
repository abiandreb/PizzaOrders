using PizzaOrders.Domain.Common;

namespace PizzaOrders.Domain.Entities.Discounts;

public class Discount : BaseEntity
{
    public string Code { get; set; }
    public string Description { get; set; }
    
    public DiscountType DiscountType { get; set; }
    
    public decimal Amount { get; set; }

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public int? MinOrderAmount { get; set; }
}

public enum DiscountType
{
    Fixed,
    Percentage
}