using RentSystem.Core.Enums;

namespace RentSystem.Core.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public State State { get; set; }
        public int AdvertId {get;set;}
        public Advert Advert { get; set; }
    }
}