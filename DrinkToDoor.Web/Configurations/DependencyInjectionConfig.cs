
using DrinkToDoor.Business.HashPassword;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Business.Services;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Repositories;
using DrinkToDoor.Data.Interfaces;

namespace DrinkToDoor.Web.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            // base
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            // services
            services.AddScoped<IAuthService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPackagingOptionService, PackagingOptionService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IKitService, KitService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IIngredientProductService, IngredientProductService>();


            // repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIngredientRepository, Data.Repositories.IngredientRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IPackagingOptionRepository, PackagingOptionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IKitRepository, KitRepository>();
            services.AddScoped<IKitIngredientRepository, KitIngredientRepository>();
            services.AddScoped<IKitProductRepository, KitProductRepository>();

            return services;
        }
    }
}