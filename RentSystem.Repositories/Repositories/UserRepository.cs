using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RentDBContext _rentDBContext;

        public UserRepository(RentDBContext rentDBContext)
        {
            _rentDBContext = rentDBContext;
        }

        public async Task CreateAsync(User user)
        {
            _rentDBContext.Users.Add(user);

            await _rentDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _rentDBContext.Users.Remove(user);

            await _rentDBContext.SaveChangesAsync();
        }

        public User? FirstOrDefault(Func<User, bool> predicate)
        {
            return _rentDBContext.Users.FirstOrDefault(predicate);
        }

        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _rentDBContext.Users.ToListAsync();
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _rentDBContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            _rentDBContext.Users.Update(user);

            await _rentDBContext.SaveChangesAsync();
        }
    }
}
