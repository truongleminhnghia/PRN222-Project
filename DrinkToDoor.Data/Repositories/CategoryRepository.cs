
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly DrinkToDoorDbContext _context;

        public CategoryRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            return 1;
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string? name, EnumCategoryType? categoryType)
        {
            IQueryable<Category> query = _context.Categories;
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }
            if (categoryType.HasValue != null)
            {
                query = query.Where(c => c.CategoryType == categoryType.Value);
            }
            query.OrderBy(c => c.Name);
            return await query.ToListAsync();
        }

        public async Task<Category?> FindById(Guid id)
        {
            return await _context.Categories.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<int> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            return 1;
        }

        public async Task<Category> FindByName(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}