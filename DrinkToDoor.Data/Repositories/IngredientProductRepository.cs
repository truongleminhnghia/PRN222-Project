using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class IngredientProductRepository : IIngredientProductRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public IngredientProductRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(IngredientProduct entity)
        {
            await _context.IngredientProducts.AddAsync(entity);
            return 1;
        }

        public async Task<IngredientProduct> Create(IngredientProduct entity)
        {
            await _context.IngredientProducts.AddAsync(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(IngredientProduct entity)
        {
            _context.IngredientProducts.Remove(entity);
            return true;
        }

        public async Task<IEnumerable<IngredientProduct>> GetAllAsync(
            string? name,
            Guid? ingredientId
        )
        {
            IQueryable<IngredientProduct> query = _context.IngredientProducts.Include(ip =>
                ip.Ingredient
            );
            if (!string.IsNullOrEmpty(name))
                query = query.Where(ip => ip.Name.Contains(name));
            if (ingredientId.HasValue)
                query = query.Where(ip => ip.IngredientId == ingredientId.Value);
            query = query.OrderBy(ip => ip.Name);
            return await query.ToListAsync();
        }

        public async Task<IngredientProduct?> FindById(Guid id)
        {
            return await _context
                .IngredientProducts
                    .Include(ip => ip.Ingredient)
                    .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(ip => ip.Id == id);
        }

        public async Task<int> UpdateAsync(IngredientProduct entity)
        {
            _context.IngredientProducts.Update(entity);
            return 1;
        }

        public async Task<IngredientProduct> FindByName(string name)
        {
            return await _context
                .IngredientProducts.Include(ip => ip.Ingredient)
                .FirstOrDefaultAsync(ip => ip.Name == name);
        }
    }
}
