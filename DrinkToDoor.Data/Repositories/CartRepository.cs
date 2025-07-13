
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
            return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
