using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class AdvertDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public DeliveryType DeliveryType { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime RentEnd { get; set; }
    }
}
