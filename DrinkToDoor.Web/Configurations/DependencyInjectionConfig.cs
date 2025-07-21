
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
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPackagingOptionService, PackagingOptionService>();


            // repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIngredientRepository, Data.Repositories.IngredientRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IPackagingOptionRepository, PackagingOptionRepository>();

            return services;
        }
    }
}