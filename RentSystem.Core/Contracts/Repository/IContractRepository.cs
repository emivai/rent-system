using RentSystem.Core.Entities;

namespace RentSystem.Core.Contracts.Repository
{
    public interface IContractRepository
    {
        Task<ICollection<Contract>> GetAllAsync();
        Task<Contract?> GetAsync(int id);
        Task CreateAsync(Contract contract);
    }
}
