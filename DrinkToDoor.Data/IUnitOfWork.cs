using DrinkToDoor.Data.Interfaces;

namespace DrinkToDoor.Data
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        ICategoryRepository Categories { get; }
        IIngredientProductRepository IngredientProducts { get; }
        IImageRepository Images { get; }
        IIngredientRepository Ingredients { get; }

        ICartItemRepository CartItems { get; }

        ICartRepository Carts { get; }

        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IPackagingOptionRepository PackagingOptions { get; }
        IPaymentRepository Payments { get; }
        IKitRepository Kits { get; }
        IKitIngredientRepository KitIngredients { get; }
        IKitProductRepository KitProducts { get; }

        IMessageRepository Messages { get; }
        Task SaveChangesAsync();
        public Task<int> SaveChangesWithTransactionAsync();
    }
}
