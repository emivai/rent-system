using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Exceptions;

namespace RentSystem.Services.Services
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ContractService(IContractRepository contractRepository, IUserRepository userRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _contractRepository = contractRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(ContractDTO contractDTO, int userId)
        {
            var renter = await _userRepository.GetAsync(userId) ?? throw new NotFoundException("User", userId);

            var item = await _itemRepository.GetAsync(contractDTO.ItemId) ?? throw new NotFoundException("Item", contractDTO.ItemId);

            var owner = await _userRepository.GetAsync(item.User.Id) ?? throw new NotFoundException("User", item.User.Id);

            var contract = _mapper.Map<Contract>(contractDTO);

            contract.RenterId = renter.Id;
            contract.Renter = renter;
            contract.OwnerId = owner.Id;
            contract.Owner = owner;
            contract.ItemId = item.Id;
            contract.Item = item;

            await _contractRepository.CreateAsync(contract);
        }

        public async Task<ICollection<GetContractDTO>> GetAllAsync()
        {
            var contracts = await _contractRepository.GetAllAsync();

            return _mapper.Map<List<GetContractDTO>>(contracts);
        }

        public async Task<GetContractDTO> GetAsync(int id)
        {
            var contract = await _contractRepository.GetAsync(id);

            if (contract == null) throw new NotFoundException("Contract", id);

            return _mapper.Map<GetContractDTO>(contract);
        }
    }
}
