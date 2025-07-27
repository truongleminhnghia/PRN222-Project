using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class KitProductRepository : IKitProductRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public KitProductRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(KitProduct kitProduct)
        {
            await _context.KitProducts.AddAsync(kitProduct);
            return 1;
        }

        public async Task<bool> Delete(KitProduct kitProduct)
        {
            _context.KitProducts.Remove(kitProduct);
            return true;
        }

        public async Task<IEnumerable<KitProduct>> FindAll()
        {
            return await _context.KitProducts.ToListAsync();
        }

        public async Task<KitProduct?> FindById(Guid id)
        {
            return await _context.KitProducts.FirstOrDefaultAsync(kp => kp.Id == id);
        }

        public async Task<bool> Update(KitProduct kitProduct)
        {
            _context.KitProducts.Update(kitProduct);
            return true;
        }
    }
}