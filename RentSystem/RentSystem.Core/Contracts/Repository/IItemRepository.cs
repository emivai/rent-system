using RentSystem.Core.Entities;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IItemRepository
    {
        Task<ICollection<Item>> GetAllAsync();
        Task<Item?> GetAsync(int id);
        Task CreateAsync(Item item);
        Task UpdateAsync(Item item);
        Task DeleteAsync(Item item);
    }
}
