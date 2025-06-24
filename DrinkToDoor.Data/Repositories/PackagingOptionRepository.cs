
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Entities;
using DrinkToDoor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data.Repositories
{
    public class PackagingOptionRepository : IPackagingOptionRepository
    {
        private readonly DrinkToDoorDbContext _context;

        public PackagingOptionRepository(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public async Task<int> Add(PackagingOption packagingOption)
        {
            await _context.PackagingOptions.AddAsync(packagingOption);
            return 1;
        }

        public async Task<bool> Delete(PackagingOption packagingOption)
        {
            _context.PackagingOptions.Remove(packagingOption);
            return true;
        }

        public Task<IEnumerable<PackagingOption>> FindAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PackagingOption?> FindById(Guid id)
        {
            return await _context.PackagingOptions.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> Update(PackagingOption packagingOption)
        {
            _context.PackagingOptions.Update(packagingOption);
            return 1;
        }
    }
}