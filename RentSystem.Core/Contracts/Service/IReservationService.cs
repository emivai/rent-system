using RentSystem.Core.DTOs;

namespace RentSystem.Core.Contracts.Service
{
    public interface IReservationService
    {
        Task<ICollection<GetReservationDTO>> GetAllAsync();
        Task<GetReservationDTO?> GetAsync(int id);
        Task CreateAsync(ReservationDTO reservation, int userId);
        Task UpdateAsync(int id, ReservationDTO reservation);
        Task DeleteAsync(int id);
    }
}
