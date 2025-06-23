using DrinkToDoor.Data.Interfaces;
using DrinkToDoor.Data;
using DrinkToDoor.BLL.Interfaces;
using DrinkToDoor.BLL.Sercvices;
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


            // repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIngredientRepository, DrinkToDoor.Data.Repositories.IngredientRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();

            return services;
        }
    }
}
