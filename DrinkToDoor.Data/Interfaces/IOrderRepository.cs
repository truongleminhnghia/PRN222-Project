using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> AddAsync(Order entity);
        Task<int> UpdateAsync(Order entity);
        Task<bool> DeleteAsync(Order entity);
        Task<Order?> FindById(Guid id);
        Task<IEnumerable<Order>> GetAllAsync(Guid? userId, EnumOrderStatus? status);
        Task<IEnumerable<Order>> FindByYear(int year);
        Task<IEnumerable<Order>> FindByMonthOfYear(int month, int year);
    }
}
