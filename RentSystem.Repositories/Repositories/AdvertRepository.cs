using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Repositories.Repositories
{
    public class AdvertRepository : IAdvertRepository
    {
        private readonly RentDBContext _rentDBContext;

        public AdvertRepository(RentDBContext rentDBContext)
        {
            _rentDBContext = rentDBContext;
        }

        public async Task<ICollection<Advert>> GetAllAsync(Category? category)
        {

            if (category.HasValue)
            {
                return await _rentDBContext.Adverts.Where(x => x.Items.Any(x => x.Category == category)).Include(x => x.Items).Include(x => x.User).ToListAsync();
            }

            return await _rentDBContext.Adverts.Include(x => x.Items).Include(x => x.User).ToListAsync();
        }

        public async Task<Advert?> GetAsync(int id)
        {
            return await _rentDBContext.Adverts.Include(x => x.Items).Include(x => x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
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
