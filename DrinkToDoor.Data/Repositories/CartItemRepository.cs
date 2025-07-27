
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public CartItemRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
            return true;
        }

        public async Task<bool> UpdateQuantityAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem?> FindByIdAsync(Guid id)
        {
            return await _context.CartItems
                .Include(ci => ci.IngredientProduct)
                .ThenInclude(ci => ci.Ingredient)
                .ThenInclude(ci => ci.Images)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<bool> DeleteAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            return true;
        }
    }
}
