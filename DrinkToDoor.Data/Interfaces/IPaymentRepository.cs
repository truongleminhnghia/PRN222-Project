

using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IPaymentRepository
    {
        Task<int> CreatePayment(Payment payment);
        Task<Payment?> FindById(Guid id);
        Task<Payment?> FindByCode(string code);
        Task<bool> Update(Payment payment);
        Task<IEnumerable<Payment>> FindAllAsync();
        Task<IEnumerable<Payment>> FindParams(EnumPaymentMethod? method, decimal? minPrice, decimal? maxPrice,
                                                            EnumPaymentStatus? status, DateTime? fromDate, DateTime? toDate,
                                                            EnumCurrency? currency, Guid? userId);
        Task<IEnumerable<Payment>> FindAll(EnumPaymentMethod? method, Guid? userId, EnumPaymentStatus? status, DateTime? fromDate, DateTime? toDate);
    }
}