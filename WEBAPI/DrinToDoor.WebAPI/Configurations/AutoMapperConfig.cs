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
            return services;
        }
    }
}
