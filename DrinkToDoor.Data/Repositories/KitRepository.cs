using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class KitRepository : IKitRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public KitRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Kit kit)
        {
            await _context.Kits.AddAsync(kit);
            return 1;
        }

        public async Task<bool> Delete(Kit kit)
        {
            _context.Kits.Remove(kit);
            return true;
        }

        public async Task<IEnumerable<Kit>> FindAll()
        {
            return await _context.Kits
                                .Include(k => k.KitIngredients)
                                .ThenInclude(ki => ki.Ingredient)
                                .ThenInclude(ki => ki.Images)
                                .ToListAsync();
        }

        public async Task<Kit?> FindById(Guid id)
        {
            return await _context.Kits
                                .Include(k => k.KitIngredients)
                                .ThenInclude(ki => ki.Ingredient)
                                .ThenInclude(ki => ki.Images)
                                .FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<Kit?> FindByName(string name)
        {
            return await _context.Kits
                                .Include(k => k.KitIngredients)
                                .FirstOrDefaultAsync(k => k.Name == name);
        }

        public async Task<IEnumerable<Kit>> FindParams(string? name, double? minPrice, double? maxPrice, EnumStatus? stauts)
        {
            IQueryable<Kit> query = _context.Kits.Include(k => k.KitIngredients);
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(k => k.Name.Contains(name));
            }
            if (minPrice.HasValue)
            {
                query = query.Where(k => k.TotalAmount >= (decimal)minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(k => k.TotalAmount <= (decimal)maxPrice.Value);
            }
            if (stauts.HasValue)
            {
                query = query.Where(k => k.Status == stauts);
            }
            query = query.OrderByDescending(k => k.CreatedAt);
            return await query.ToListAsync();
        }

        public async Task<bool> Update(Kit kit)
        {
            _context.Kits.Update(kit);
            return true;
        }
    }
}