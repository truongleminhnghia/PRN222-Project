
using DrinkToDoor.Business.HashPassword;
using DrinkToDoor.Business.Interfaces;
using DrinkToDoor.Business.Services;
using DrinkToDoor.Data;
using DrinkToDoor.Data.Repositories;
using DrinkToDoor.Data.Repositories.Interfaces;

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


            // repository
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}