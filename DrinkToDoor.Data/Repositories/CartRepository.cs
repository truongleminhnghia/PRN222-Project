
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public CartRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            return 1;
        }

        public async Task<bool> Delete(Cart cart)
        {
            _context.Carts.Remove(cart);
            return true;
        }

        public async Task<Cart?> FindById(Guid cartId)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
        }

        public async Task<Cart?> FindByUserId(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.IngredientProduct)
                    .ThenInclude(ip => ip.Ingredient)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<Cart>> GetAllWithParamsAsync(Guid? userId, string? sortBy, bool isDescending, int pageNumber, int pageSize)
        {
            var query = _context.Carts
                .Include(c => c.User)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.IngredientProduct)
                    .ThenInclude(ip => ip.Ingredient)
                .AsQueryable();
            if (userId.HasValue)
            {
                query = query.Where(c => c.UserId == userId.Value);
            }

            query = sortBy?.ToLower() switch
            {
                "userid" => isDescending ? query.OrderByDescending(c => c.UserId) : query.OrderBy(c => c.UserId),
                "id" => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                _ => isDescending ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id)
            };

            return await query.ToListAsync();

        }
    }
}
