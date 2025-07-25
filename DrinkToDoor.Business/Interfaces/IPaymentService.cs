
using DrinkToDoor.Business.Dtos.Requests;
using Net.payOS.Types;
namespace DrinkToDoor.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentAsync(PaymentRequest request);
        Task<bool> VerifyPaymentAsync(WebhookType type);
    }
}