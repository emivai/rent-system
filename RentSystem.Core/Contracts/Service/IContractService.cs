using RentSystem.Core.DTOs;

namespace RentSystem.Core.Contracts.Service
{
    public interface IContractService
    {
        Task<ICollection<GetContractDTO>> GetAllAsync();
        Task<GetContractDTO> GetAsync(int id);
        Task CreateAsync(ContractDTO item, int userId);
    }
}
