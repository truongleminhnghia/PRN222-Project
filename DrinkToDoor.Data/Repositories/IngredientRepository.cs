
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class IngredientRepository : IIngredientRepository
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
            return await _context.Ingredients
                                    .Include(i => i.Images)
                                    .Include(i => i.Category)
                                    .Include(i => i.PackagingOptions)
                                    .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Ingredient>> FindByName(string? name)
        {
            return await _context.Ingredients
                                 .Include(i => i.Images)
                                 .Include(i => i.Category)
                                 .Include(i => i.PackagingOptions)
                                 .Where(i => string.IsNullOrEmpty(name) || i.Name.Contains(name))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync(string? keyword, string? name, Guid? categoryId, decimal? minPrice, decimal? maxPrice,
                                                       decimal? minCost, decimal? maxCost, int? minQuantity, int? maxQuantity, EnumStatus? status)
        {
            IQueryable<Ingredient> query = _context.Ingredients
                                                        .Include(i => i.Images)
                                                        .Include(i => i.Category)
                                                        .Include(i => i.PackagingOptions);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(i =>
                    EF.Functions.Like(i.Name, $"{keyword}") ||
                    EF.Functions.Like(i.Description, $"{keyword}")
                );
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(i => i.Name.Contains(name));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(i => i.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(i => i.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(i => i.Price <= maxPrice.Value);
            }

            if (minCost.HasValue)
            {
                query = query.Where(i => i.Cost >= minCost.Value);
            }

            if (maxCost.HasValue)
            {
                query = query.Where(i => i.Cost <= maxCost.Value);
            }

            if (minQuantity.HasValue)
            {
                query = query.Where(i => i.StockQty >= minQuantity.Value);
            }

            if (maxQuantity.HasValue)
            {
                query = query.Where(i => i.StockQty <= maxQuantity.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(i => i.Status == status.Value);
            }

            return await query
                .OrderByDescending(i => i.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> UpdateAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            return 1;
        }
    }
}