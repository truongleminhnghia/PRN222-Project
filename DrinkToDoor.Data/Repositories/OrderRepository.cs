using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public OrderRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Order entity)
        {
            var now = DateTime.UtcNow;
            entity.CreatedAt = now;
            entity.UpdatedAt = now;
            await _context.Orders.AddAsync(entity);
            return 1;
        }

        public async Task<bool> DeleteAsync(Order entity)
        {
            _context.Orders.Remove(entity);
            return true;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(Guid? userId, EnumOrderStatus? status)
        {
            IQueryable<Order> query = _context.Orders
                                                .Include(o => o.User)
                                                .Include(o => o.OrderDetails)
                                                    .ThenInclude(od => od.IngredientProduct)
                                                    .ThenInclude(od => od.Ingredient)
                                                    .ThenInclude(od => od.Images)
                                                .Include(o => o.Payments);

            if (userId.HasValue)
                query = query.Where(o => o.UserId == userId.Value);
            if (status.HasValue)
                query = query.Where(o => o.Status == status.Value);

            query = query.OrderByDescending(o => o.OrderDate);
            return await query.ToListAsync();
        }

        public async Task<Order?> FindById(Guid id)
        {
            return await _context
                .Orders.Include(o => o.User)
                .Include(o => o.OrderDetails)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> UpdateAsync(Order entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(entity);
            return 1;
        }
    }
}
