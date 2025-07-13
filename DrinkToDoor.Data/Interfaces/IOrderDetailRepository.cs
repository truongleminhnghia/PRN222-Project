using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<int> AddAsync(OrderDetail entity);
        Task<int> UpdateAsync(OrderDetail entity);
        Task<bool> DeleteAsync(OrderDetail entity);
        Task<OrderDetail?> FindById(Guid id);
        Task<IEnumerable<OrderDetail>> GetAllAsync(
            Guid? orderId,
            Guid? kitProductId,
            Guid? ingredientProductId
        );
    }
}
