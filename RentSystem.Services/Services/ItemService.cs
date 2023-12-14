using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Exceptions;

namespace RentSystem.Services.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAdvertRepository _advertRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IAdvertRepository advertRepository, IUserRepository userRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _advertRepository = advertRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetItemDTO>> GetAllAsync()
        {
            var items = await _itemRepository.GetAllAsync();

            return _mapper.Map<List<GetItemDTO>>(items);
        }

        public async Task<GetItemDTO> GetAsync(int id)
        {
            var item = await _itemRepository.GetAsync(id);

            if (item == null)
            {
                throw new NotFoundException("Item", id);
            }

            return _mapper.Map<GetItemDTO>(item);
        }

        public async Task CreateAsync(ItemDTO itemDTO, int userId)
        {
            var user = await _userRepository.GetAsync(userId) ?? throw new NotFoundException("User was not found");

            var item = _mapper.Map<Item>(itemDTO);

            item.UserId = userId;
            item.User = user;

            await _itemRepository.CreateAsync(item);
        }

        public async Task UpdateAsync(int id, ItemDTO itemDTO)
        {
            var item = await _itemRepository.GetAsync(id) ?? throw new NotFoundException("Item", id);

            item.Name = itemDTO.Name;
            item.Price = itemDTO.Price;
            item.Category = itemDTO.Category;
            item.State = itemDTO.State;

            var advert = await _advertRepository.GetAsync(itemDTO.AdvertId);

            if (advert == null) throw new NotFoundException("No such advert exists");

            item.AdvertId = advert.Id;

            await _itemRepository.UpdateAsync(item);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _itemRepository.GetAsync(id);

            if (item == null) throw new NotFoundException("Item", id);

            await _itemRepository.DeleteAsync(item);
        }
    }
}
