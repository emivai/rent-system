using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentSystem.Core.Contracts.Service;
using RentSystem.Services.Services;

namespace RentSystem.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IAdvertService, AdvertService>();

            services.AddAutoMapper(typeof(MappingProfiles.ItemMappingProfile));

            return services;
        }
    }
}
