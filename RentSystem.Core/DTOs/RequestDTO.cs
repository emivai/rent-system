using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class RequestDTO
    {
        public Category Category { get; set; }
        public int Count { get; set; }
    }
}
