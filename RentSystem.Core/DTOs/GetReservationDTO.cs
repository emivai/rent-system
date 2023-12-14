using RentSystem.Core.Entities;

namespace RentSystem.Core.DTOs
{
    public class GetReservationDTO
    {
        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double Price { get; set; }
        public GetReservationItemDTO Item { get; set; }
        public UserDTO User { get; set; }
    }

    public class GetReservationItemDTO
    {
        public int Id { get; set; }
        public int Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int State { get; set; }
        public UserDTO User {get;set;}
    }
}
