
using DrinkToDoor.Data.Interfaces;

namespace DrinkToDoor.Data
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }

        ISupplierRepository Suppliers { get; }
        IImageRepository Images { get; }
        IIngredientRepository Ingredients { get; }

        ICartItemRepository CartItems { get; }

        ICartRepository Carts {  get; }

        IPackagingOptionRepository PackagingOptions { get; }
        Task SaveChangesAsync();
        public Task<int> SaveChangesWithTransactionAsync();
    }
}