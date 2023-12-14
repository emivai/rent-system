using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Services.Handlers;
using RentSystem.Services.Services;
using RentSystem.Services.Validations;

namespace RentSystem.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IAdvertService, AdvertService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IReservationService, ReservationService>();
            services.AddTransient<IContractService, ContractService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<ITokenManager, TokenManager>();

            services.AddSingleton<IAuthorizationHandler, SameUserAuthorizationHandler>();

            services.AddScoped<IValidator<AdvertDTO>, AdvertValidator>();
            services.AddScoped<IValidator<ItemDTO>, ItemValidator>();
            services.AddScoped<IValidator<RegisterUserDTO>, UserValidator>();

            services.AddHostedService<BackgroundWorkerService>();

            services.AddAutoMapper(typeof(MappingProfiles.ItemMappingProfile));

            return services;
        }
    }
}
