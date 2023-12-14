using RentSystem.Core.Entities;

namespace RentSystem.Core.DTOs
{
    public class GetContractDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int RentLength { get; set; }
        public double Price { get; set; }
        public GetItemDTO Item { get; set; }
        public UserDTO Renter { get; set; }
        public UserDTO Owner { get; set; }
    }
}
