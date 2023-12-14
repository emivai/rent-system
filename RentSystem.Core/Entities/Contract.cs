using RentSystem.Core.Contracts.Model;

namespace RentSystem.Core.Entities
{
    public class Contract
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int RentLength { get; set; }
        public double Price { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int RenterId { get; set; }
        public User Renter { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
