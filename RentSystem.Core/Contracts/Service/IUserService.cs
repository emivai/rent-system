using RentSystem.Core.DTOs;


namespace RentSystem.Core.Contracts.Service
{
    public interface IUserService
    {
        Task<ICollection<UserDTO>> GetAllAsync();
        Task<UserDTO?> GetAsync(int id);
        Task<SuccessfullLoginDTO> LoginAsync(LoginUserDTO user);
        Task CreateAsync(RegisterUserDTO user);
        Task UpdateAsync(int id, UpdateUserDTO user);
        Task DeleteAsync(int id);
    }
}
