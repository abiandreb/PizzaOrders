using System.Threading.Tasks;
using PizzaOrders.Application.DTOs;

namespace PizzaOrders.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> ProcessPayment(PaymentRequestDto paymentRequest);
    }
}
