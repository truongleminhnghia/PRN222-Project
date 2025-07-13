using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public OrderDetailRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(OrderDetail entity)
        {
            await _context.OrderDetails.AddAsync(entity);
            return 1;
        }

        public async Task<bool> DeleteAsync(OrderDetail entity)
        {
            _context.OrderDetails.Remove(entity);
            return true;
        }

        public async Task<IEnumerable<OrderDetail>> GetAllAsync(
            Guid? orderId,
            Guid? kitProductId,
            Guid? ingredientProductId
        )
        {
            IQueryable<OrderDetail> query = _context
                .OrderDetails.Include(od => od.Order)
                .Include(od => od.KitProduct)
                .Include(od => od.IngredientProduct);

            if (orderId.HasValue)
                query = query.Where(od => od.OrderId == orderId.Value);
            if (kitProductId.HasValue)
                query = query.Where(od => od.KitProductId == kitProductId.Value);
            if (ingredientProductId.HasValue)
                query = query.Where(od => od.IngredientProductId == ingredientProductId.Value);

            query = query.OrderBy(od => od.Id);
            return await query.ToListAsync();
        }

        public async Task<OrderDetail?> FindById(Guid id)
        {
            return await _context
                .OrderDetails.Include(od => od.Order)
                .Include(od => od.KitProduct)
                .Include(od => od.IngredientProduct)
                .FirstOrDefaultAsync(od => od.Id == id);
        }

        public async Task<int> UpdateAsync(OrderDetail entity)
        {
            _context.OrderDetails.Update(entity);
            return 1;
        }
    }
}
