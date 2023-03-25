using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly RentDBContext _rentDBContext;

        public ItemRepository(RentDBContext rentDBContext) 
        {
            _rentDBContext = rentDBContext;
        }

        public async Task<ICollection<Item>> GetAllAsync()
        {
            return await _rentDBContext.Items.ToListAsync();
        }

        public async Task<Item?> GetAsync(int id)
        {
            return await _rentDBContext.Items.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task CreateAsync(Item item)
        {
            _rentDBContext.Items.Add(item);

            await _rentDBContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Item item)
        {
            _rentDBContext.Items.Update(item);

            await _rentDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Item item)
        {
            _rentDBContext.Items.Remove(item);

            await _rentDBContext.SaveChangesAsync();
        }
    }
}
