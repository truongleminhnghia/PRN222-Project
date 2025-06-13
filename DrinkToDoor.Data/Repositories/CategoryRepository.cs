
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.enums;
using DrinkToDoor.Data.Repositories.Interfaces;
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

        public Task<IEnumerable<Category>> GetAllAsync(EnumCategoryType? categoryType)
        {
            throw new NotImplementedException();
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
    }
}