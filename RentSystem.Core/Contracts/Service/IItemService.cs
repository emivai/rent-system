using RentSystem.Core.DTOs;

namespace RentSystem.Core.Contracts.Service
{
    public interface IItemService
    {
        Task<ICollection<GetItemDTO>> GetAllAsync();
        Task<GetItemDTO> GetAsync(int id);
        Task CreateAsync(ItemDTO item, int userId);
        Task UpdateAsync(int id, ItemDTO item);
        Task DeleteAsync(int id);
    }
}
