using DrinkToDoor.Data.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.Services;
using DrinkToDoor.BLL.HashPasswords;
using DrinkToDoor.Data.Repositories;

namespace DrinToDoor.WebAPI.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services)
        {
            // base
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            // services
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IPackagingOptionService, PackagingOptionService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IIngredientService, IngredientService>();


            // repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IIngredientRepository, DrinkToDoor.Data.Repositories.IngredientRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IPackagingOptionRepository, PackagingOptionRepository>();

            return services;
        }
    }
}
