using RentSystem.Core.Contracts.Model;

namespace RentSystem.Core.Entities
{
    public class Reservation : IUserOwnedResource
    {
        public int Id { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public double Price { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
