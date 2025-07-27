
using DrinkToDoor.Business.Dtos.Requests;
using DrinkToDoor.Business.Dtos.Responses;
using DrinkToDoor.Data.enums;
namespace DrinkToDoor.Business.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentAsync(PaymentRequest request);
        Task<bool> VerifyPaymentAsync(string code, string status, string orderCode, bool cancel);
        Task<Tuple<IEnumerable<PaymentResponse>, int>> GetAllAsync(int pageCurrent, int pageSize);
        Task<Tuple<IEnumerable<PaymentResponse>, int>> GetParams(EnumPaymentMethod? method, decimal? minPrice, decimal? maxPrice,
                                                            EnumPaymentStatus? status, DateTime? fromDate, DateTime? toDate,
                                                            EnumCurrency? currency, Guid? userId, int pageCurrent, int pageSize);
    }
}