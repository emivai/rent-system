using Microsoft.EntityFrameworkCore;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Entities;

namespace RentSystem.Repositories.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly RentDBContext _rentDBContext;

        public ReservationRepository(RentDBContext rentDBContext)
        {
            _rentDBContext = rentDBContext;
        }

        public async Task<ICollection<Reservation>> GetAllAsync()
        {
            return await _rentDBContext.Reservations.Include(x => x.User).Include(x => x.Item).ThenInclude(x => x.User).ToListAsync();
        }

        public async Task<Reservation?> GetAsync(int id)
        {
            return await _rentDBContext.Reservations.Include(x => x.User).Include(x => x.Item).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Reservation>> GetByTimeIntervalAsync(DateTime dateFrom, DateTime dateTo)
        {
            return await _rentDBContext.Reservations.Where(r =>
                (dateFrom >= r.DateFrom && dateFrom <= r.DateTo)
                ||
                (dateTo >= r.DateFrom && dateTo <= r.DateTo)
            ).ToListAsync();
        }

        public async Task CreateAsync(Reservation reservation)
        {
            _rentDBContext.Reservations.Add(reservation);

            await _rentDBContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _rentDBContext.Reservations.Update(reservation);

            await _rentDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reservation reservation)
        {
            _rentDBContext.Reservations.Remove(reservation);

            await _rentDBContext.SaveChangesAsync();
        }
    }
}
