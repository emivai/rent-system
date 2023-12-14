using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class GetRequestDTO
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public int Count { get; set; }
        public bool IsAvailable { get; set; }
        public UserDTO User { get; set; }
    }
}
