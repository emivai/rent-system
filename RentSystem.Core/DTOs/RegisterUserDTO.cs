using RentSystem.Core.Enums;

namespace RentSystem.Core.DTOs
{
    public class RegisterUserDTO
    {
        public Role Role { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string City { get; set; } = "";
        public string HouseNumber { get; set; } = "";
        public string PostCode { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }
}
