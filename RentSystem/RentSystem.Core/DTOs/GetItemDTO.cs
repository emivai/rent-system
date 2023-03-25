using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class GetItemDTO
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public State State { get; set; }
    }
}
