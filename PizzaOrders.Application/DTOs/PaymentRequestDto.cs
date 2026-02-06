using System.ComponentModel.DataAnnotations;

namespace PizzaOrders.Application.DTOs;

public class PaymentRequestDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Valid OrderId is required")]
    public int OrderId { get; set; }

    [Required]
    [Range(0.01, 100000, ErrorMessage = "Amount must be between 0.01 and 100000")]
    public decimal Amount { get; set; }
}
