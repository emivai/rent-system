using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class ItemDTO
    {
        public Category Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public State State { get; set; }
        public int AdvertId { get; set; }
    }
}
