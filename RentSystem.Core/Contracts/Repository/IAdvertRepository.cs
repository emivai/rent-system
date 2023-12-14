using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IAdvertRepository
    {
        Task<ICollection<Advert>> GetAllAsync(Category? category);
        Task<Advert?> GetAsync(int id);
        Task CreateAsync(Advert item);
        Task UpdateAsync(Advert item);
        Task DeleteAsync(Advert item);
    }
}
