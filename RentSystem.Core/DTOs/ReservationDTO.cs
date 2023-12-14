using RentSystem.Core.Entities;

namespace RentSystem.Core.DTOs
{
    public class ReservationDTO
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double Price { get; set; }
        public int ItemId { get; set; }
    }
}
