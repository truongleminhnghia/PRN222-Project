
using DrinkToDoor.Data.Entities;

namespace DrinkToDoor.Data.Interfaces
{
    public interface IPackagingOptionRepository
    {
        Task<int> Add(PackagingOption packagingOption);
        Task<int> Update(PackagingOption packagingOption);
        Task<bool> Delete(PackagingOption packagingOption);
        Task<PackagingOption?> FindById(Guid id);
        Task<IEnumerable<PackagingOption>> FindAll();
    }
}