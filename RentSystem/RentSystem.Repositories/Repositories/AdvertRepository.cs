using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories.Repositories
{
    public class AdvertRepository : IAdvertRepository
    {
        private readonly RentDBContext _rentDBContext;

        public AdvertRepository(RentDBContext rentDBContext)
        {
            _rentDBContext = rentDBContext;
        }

        public async Task<ICollection<Advert>> GetAllAsync()
        {
            return await _rentDBContext.Adverts.Include(x => x.Items).ToListAsync();
        }

        public async Task<Advert?> GetAsync(int id)
        {
            return await _rentDBContext.Adverts.Include(x => x.Items).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task CreateAsync(Advert advert)
        {
            _rentDBContext.Adverts.Add(advert);

            await _rentDBContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Advert advert)
        {
            _rentDBContext.Adverts.Update(advert);

            await _rentDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Advert advert)
        {
            _rentDBContext.Adverts.Remove(advert);

            await _rentDBContext.SaveChangesAsync();
        }
    }
}
