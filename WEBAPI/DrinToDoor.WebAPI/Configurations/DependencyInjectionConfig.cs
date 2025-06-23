using DrinkToDoor.Data.Repositories.Interfaces;
using DrinkToDoor.Data.Repositories;
using DrinkToDoor.Data;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.Sercvices;
using DrinkToDoor.BLL.HashPasswords;

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


            // repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            return services;
        }
    }
}
