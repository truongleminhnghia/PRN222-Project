using DrinkToDoor.Data.Interfaces;

namespace DrinkToDoor.Data
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IIngredientProductRepository IngredientProducts { get; }

        ISupplierRepository Suppliers { get; }
        IImageRepository Images { get; }
        IIngredientRepository Ingredients { get; }

        ICartItemRepository CartItems { get; }

        ICartRepository Carts { get; }

        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IPackagingOptionRepository PackagingOptions { get; }
        Task SaveChangesAsync();
        public Task<int> SaveChangesWithTransactionAsync();
    }
}
