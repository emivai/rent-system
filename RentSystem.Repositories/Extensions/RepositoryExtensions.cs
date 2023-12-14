using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Repositories.Repositories;

namespace RentSystem.Repositories.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("postgres");

            services.AddDbContext<RentDBContext>(options => options.UseNpgsql(connectionString));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IAdvertRepository, AdvertRepository>();
            services.AddTransient<IReservationRepository, ReservationRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<IRequestRepository, RequestRepository>();

            return services;
        }
    }
}
