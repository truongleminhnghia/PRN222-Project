using DrinkToDoor.Data.Context;
using DrinkToDoor.Data.Interfaces;
using DrinkToDoor.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DrinkToDoor.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DrinkToDoorDbContext _context;
        private IUserRepository? _userRepository;
        private ICategoryRepository? _categoryRepository;
        private ISupplierRepository? _supplierRepository;
        private IPackagingOptionRepository? _packagingOptionRepository;
        private IImageRepository? _imageRepository;
        private IIngredientRepository? _ingredientRepository;
        private ICartItemRepository? _cartItemRepository;
        private ICartRepository? _cartRepository;
        private IIngredientProductRepository? _ingredientProductRepository;
        private IOrderRepository? _orderRepository;
        private IOrderDetailRepository? _orderDetailRepository;
        private IMessageRepository? _messageRepository;

        public UnitOfWork(DrinkToDoorDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public ICategoryRepository Categories =>
            _categoryRepository ??= new CategoryRepository(_context);
        public ISupplierRepository Suppliers =>
            _supplierRepository ??= new SupplierRepository(_context);
        public IPackagingOptionRepository PackagingOptions =>
            _packagingOptionRepository ??= new PackagingOptionRepository(_context);

        public IImageRepository Images => _imageRepository ??= new ImageRepository(_context);

        public IIngredientRepository Ingredients =>
            _ingredientRepository ??= new Repositories.IngredientRepository(_context);
        public IIngredientProductRepository IngredientProducts =>
            _ingredientProductRepository ??= new IngredientProductRepository(_context);
        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);

        public IOrderDetailRepository OrderDetails =>
            _orderDetailRepository ??= new OrderDetailRepository(_context);

        public ICartItemRepository CartItems =>
            _cartItemRepository ??= new CartItemRepository(_context);

        public ICartRepository Carts => 
            _cartRepository ??= new CartRepository(_context);

        public IMessageRepository Messages => 
            _messageRepository ??= new MessageRepository(_context);
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
                await using var transaction = await _context.Database.BeginTransactionAsync();
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
