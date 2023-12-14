using RentSystem.Core.Contracts.Model;
using RentSystem.Core.Enums;

namespace RentSystem.Core.Entities
{
    public class Advert : IUserOwnedResource
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public DeliveryType DeliveryType { get; set; }
        public DateTime RentStart {get;set;}
        public DateTime RentEnd { get; set; }
        public virtual List<Item> Items { get; set; } = new List<Item>();
        public int UserId { get; set; }
        public User User { get; set; } = new User();
    }
}
