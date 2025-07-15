
using DrinkToDoor.Business.Mappers;

namespace DrinkToDoor.Web.Configurations
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfiguration(
            this IServiceCollection services
        )
        {
            services.AddAutoMapper(typeof(UserMapper));
            services.AddAutoMapper(typeof(IngredientMapper));
            services.AddAutoMapper(typeof(ImageMapper));
            services.AddAutoMapper(typeof(SupplierMapper));
            services.AddAutoMapper(typeof(PackageOptionMapper));
            return services;
        }
    }
}