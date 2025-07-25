
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
            services.AddAutoMapper(typeof(CartMapper));
            services.AddAutoMapper(typeof(CartItemMapper));
            services.AddAutoMapper(typeof(IngredientProductMapper));
            services.AddAutoMapper(typeof(OrderMapper));
            services.AddAutoMapper(typeof(OrderDetailMapper));
            services.AddAutoMapper(typeof(PaymentMapper));
            return services;
        }
    }
}