using DrinkToDoor.BLL.Mappers;

namespace DrinToDoor.WebAPI.Configurations
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfiguration(
            this IServiceCollection services
        )
        {
            services.AddAutoMapper(typeof(UserMapper));
            services.AddAutoMapper(typeof(CategoryMapper));
            services.AddAutoMapper(typeof(SupplierMapper));
            services.AddAutoMapper(typeof(PackagingOptionMapper));
            services.AddAutoMapper(typeof(ImageMapper));
            services.AddAutoMapper(typeof(IngredientMapper));
            services.AddAutoMapper(typeof(IngredientProductMapper));
            return services;
        }
    }
}
