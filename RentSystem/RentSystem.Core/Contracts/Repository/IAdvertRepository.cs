using RentSystem.Core.Entities;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IAdvertRepository
    {
        Task<ICollection<Advert>> GetAllAsync();
        Task<Advert?> GetAsync(int id);
        Task CreateAsync(Advert item);
        Task UpdateAsync(Advert item);
        Task DeleteAsync(Advert item);
    }
}
