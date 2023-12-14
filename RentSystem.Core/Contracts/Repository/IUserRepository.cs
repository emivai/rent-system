using RentSystem.Core.Entities;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetAllAsync();
        Task<User?> GetAsync(int id);
        User? FirstOrDefault(Func<User, bool> predicate);
        Task CreateAsync(User item);
        Task UpdateAsync(User item);
        Task DeleteAsync(User item);
    }
}
