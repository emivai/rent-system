using RentSystem.Core.Entities;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IReservationRepository
    {
        Task<ICollection<Reservation>> GetAllAsync();
        Task<Reservation?> GetAsync(int id);
        Task<List<Reservation>> GetByTimeIntervalAsync(DateTime dateFrom, DateTime dateTo);
        Task CreateAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
    }
}
