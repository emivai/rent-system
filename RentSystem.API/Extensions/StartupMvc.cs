using Newtonsoft.Json;

namespace RentSystem.API.Extensions
{
    public static class StartupMvc
    {
        public static IMvcCoreBuilder ConfigureMvc(this IServiceCollection services)
        {
            return services
            .AddMvcCore()
            .AddNewtonsoftJson(
              options => {
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
             });
        }
    }
}
