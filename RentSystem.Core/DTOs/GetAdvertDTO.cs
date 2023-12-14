namespace RentSystem.Core.DTOs
{
    public class GetAdvertDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public int DeliveryType { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime RentEnd { get; set; }
        public virtual List<GetItemDTO> Items { get; set; } = new List<GetItemDTO>();
        public UserDTO User { get; set; } = new UserDTO();
    }
}
