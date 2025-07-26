
using DrinkToDoor.Business.Dtos.Requests;
using Net.payOS.Types;
namespace DrinkToDoor.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentAsync(PaymentRequest request);
        Task<bool> VerifyPaymentAsync(string code, string status, string orderCode, bool cancel);
    }
}