
using DrinkToDoor.Data.Repositories.Interfaces;

namespace DrinkToDoor.Data
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        Task SaveChangesAsync();
        public Task<int> SaveChangesWithTransactionAsync();
    }
}