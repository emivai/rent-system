using RentSystem.Core.Entities;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IRequestRepository
    {
        Task<ICollection<Request>> GetAllAsync();
        Task<ICollection<Request>> GetUnavailableAsync();
        Task<Request?> GetAsync(int id);
        Task CreateAsync(Request request);
        Task UpdateAsync(Request request);
        Task DeleteAsync(Request request);
    }
}
