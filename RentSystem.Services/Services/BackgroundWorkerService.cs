using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Enums;

namespace RentSystem.Services.Services
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BackgroundWorkerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var requestRepository = scope.ServiceProvider.GetRequiredService<IRequestRepository>();
                var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var requests = await requestRepository.GetUnavailableAsync();
                    var items = await itemRepository.GetAllAsync();

                    foreach (var request in requests)
                    {
                        if (items.Any(x => x.State == State.Available))
                        {
                            request.IsAvailable = true;
                            await requestRepository.UpdateAsync(request);
                        }
                    }
                    await Task.Delay(180000);
                }
            }
        }
    }
}
