using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Core.Contracts.Service
{
    public interface IAdvertService
    {
        Task<ICollection<GetAdvertDTO>> GetAllAsync(Category? category);
        Task<GetAdvertDTO> GetAsync(int id);
        Task CreateAsync(AdvertDTO item, int userId);
        Task UpdateAsync(int id, AdvertDTO item);
        Task DeleteAsync(int id);
    }
}
