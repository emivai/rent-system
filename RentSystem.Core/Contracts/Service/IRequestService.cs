using RentSystem.Core.DTOs;

namespace RentSystem.Core.Contracts.Service
{
    public interface IRequestService
    {
        Task<ICollection<GetRequestDTO>> GetAllAsync();
        Task<GetRequestDTO> GetAsync(int id);
        Task CreateAsync(RequestDTO request, int userId);
        Task UpdateAsync(int id, RequestDTO request);
        Task DeleteAsync(int id);
    }
}
