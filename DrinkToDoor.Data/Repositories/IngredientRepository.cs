
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class IngredientRepository :  IIngredientRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public IngredientRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Ingredient ingredient)
        {
            await _context.Ingredients.AddAsync(ingredient);
            return 1;
        }

        public async Task<bool> DeleteAsync(Ingredient ingredient)
        {
            _context.Ingredients.Remove(ingredient);
            return true;
        }

        public async Task<Ingredient?> FindById(Guid id)
        {
            return await _context.Ingredients.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<int> UpdateAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            return 1;
        }
    }
}