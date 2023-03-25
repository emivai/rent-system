using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class GetItemDTO
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string State { get; set; } = string.Empty;
    }
}
