using RentSystem.Core.Contracts.Model;
using RentSystem.Core.Enums;

namespace RentSystem.Core.Entities
{
    public class Item : IUserOwnedResource
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public State State { get; set; }
        public int? AdvertId { get; set; }
        public Advert? Advert { get; set; }
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public int? ContractId { get; set; }
        public Contract? Contract { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}