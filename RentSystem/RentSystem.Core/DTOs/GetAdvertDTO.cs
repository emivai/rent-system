using RentSystem.Core.Entities;
using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class GetAdvertDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string DeliveryType { get; set; } = string.Empty;
        public DateTime RentStart { get; set; }
        public DateTime RentEnd { get; set; }
        public virtual List<GetItemDTO> Items { get; set; } = new List<GetItemDTO>();
    }
}
