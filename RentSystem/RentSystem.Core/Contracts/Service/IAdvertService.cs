using RentSystem.Core.DTOs;

namespace RentSystem.Core.Contracts.Service
{
    public interface IAdvertService
    {
        Task<ICollection<GetAdvertDTO>> GetAllAsync();
        Task<GetAdvertDTO> GetAsync(int id);
        Task CreateAsync(AdvertDTO item);
        Task UpdateAsync(int id, AdvertDTO item);
        Task DeleteAsync(int id);
    }
}
