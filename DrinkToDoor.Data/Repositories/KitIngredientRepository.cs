
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class KitIngredientRepository : IKitIngredientRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public KitIngredientRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(KitIngredient kitIngredient)
        {
            await _context.KitIngredients.AddAsync(kitIngredient);
            return 1;
        }

        public async Task<bool> Delete(KitIngredient kitIngredient)
        {
            _context.KitIngredients.Remove(kitIngredient);
            return true;
        }

        public async Task<IEnumerable<KitIngredient>> FindAll()
        {
            return await _context.KitIngredients.ToListAsync();
        }

        public async Task<KitIngredient?> FindById(Guid id)
        {
            return await _context.KitIngredients.FirstOrDefaultAsync(ki => ki.Id == id);
        }

        public async Task<bool> Update(KitIngredient kitIngredient)
        {
            _context.KitIngredients.Update(kitIngredient);
            return true;
        }
    }
}