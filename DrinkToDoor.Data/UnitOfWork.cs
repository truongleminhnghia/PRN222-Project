
using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Repositories;
using DrinkToDoor.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DrinkToDoorDbContext _context;
        private IUserRepository? _userRepository;
        private ICategoryRepository? _categoryRepository;


        public UnitOfWork(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);


        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"SaveChanges failed: {ex.Message}", ex);
            }
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction =
                    await _context.Database.BeginTransactionAsync();
                try
                {
                    var result = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return result;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException($"Transaction failed: {ex.Message}", ex);
                }
            });
        }
    }
}