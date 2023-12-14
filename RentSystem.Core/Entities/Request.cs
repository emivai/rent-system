using RentSystem.Core.Contracts.Model;
using RentSystem.Core.Enums;

namespace RentSystem.Core.Entities
{
    public class Request : IUserOwnedResource
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public int Count { get; set; }
        public bool IsAvailable { get; set; }
        public int UserId { get; set ; }
        public User User { get; set ; }
    }
}
