
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
            return services;
        }
    }
}