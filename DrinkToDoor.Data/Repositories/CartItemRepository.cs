
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

       
    }
}
