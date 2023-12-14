using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories.Repositories
{
    internal class RequestRepository : IRequestRepository
    {
        private readonly RentDBContext _rentDBContext;

        public RequestRepository(RentDBContext rentDBContext)
        {
            _rentDBContext = rentDBContext;
        }

        public async Task<ICollection<Request>> GetAllAsync()
        {
            return await _rentDBContext.Requests.Include(x => x.User).ToListAsync();
        }

        public async Task<ICollection<Request>> GetUnavailableAsync()
        {
            return await _rentDBContext.Requests.Where(x => x.IsAvailable == false).Include(x => x.User).ToListAsync();
        }

        public async Task<Request?> GetAsync(int id)
        {
            return await _rentDBContext.Requests.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Request request)
        {
            _rentDBContext.Requests.Add(request);

            var x = await _rentDBContext.Requests.Include(x => x.User).ToListAsync();

            await _rentDBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Request request)
        {
            _rentDBContext.Requests.Update(request);

            await _rentDBContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Request request)
        {
            _rentDBContext.Requests.Remove(request);

            await _rentDBContext.SaveChangesAsync();
        }
    }
}
