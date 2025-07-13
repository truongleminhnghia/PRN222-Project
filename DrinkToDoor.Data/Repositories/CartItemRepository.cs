
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Interfaces;

namespace DrinkToDoor.Data.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public CartItemRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }
    }
}
