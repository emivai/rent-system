using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Enums;
using RentSystem.Core.Exceptions;

namespace RentSystem.Services.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly IAdvertRepository _advertRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AdvertService(IAdvertRepository advertRepository, IItemRepository itemRepository,IUserRepository userRepository, IMapper mapper)
        {
            _advertRepository = advertRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetAdvertDTO>> GetAllAsync(Category? category)
        {
            var adverts = await _advertRepository.GetAllAsync(category);

            return _mapper.Map<List<GetAdvertDTO>>(adverts);
        }

        public async Task<GetAdvertDTO> GetAsync(int id)
        {
            var advert = await _advertRepository.GetAsync(id);

            if (advert == null) throw new NotFoundException("Advert", id);


            return _mapper.Map<GetAdvertDTO>(advert);
        }

        public async Task CreateAsync(AdvertDTO advertDTO, int userId)
        {
            var user = await _userRepository.GetAsync(userId) ?? throw new NotFoundException("User was not found");

            var advert = _mapper.Map<Advert>(advertDTO);

            advert.UserId = userId;
            advert.User = user;

            await _advertRepository.CreateAsync(advert);
        }

        public async Task UpdateAsync(int id, AdvertDTO advertDTO)
        {
            var advert = await _advertRepository.GetAsync(id);

            if (advert == null) throw new NotFoundException("Advert", id);

            advert.Title = advertDTO.Title;
            advert.Description = advertDTO.Description;
            advert.ImageUrl = advertDTO.ImageUrl;
            advert.VideoUrl = advertDTO.VideoUrl;
            advert.RentStart = advertDTO.RentStart;
            advert.RentEnd = advertDTO.RentEnd;
            advert.DeliveryType = advertDTO.DeliveryType;

            await _advertRepository.UpdateAsync(advert);
        }

        public async Task DeleteAsync(int id)
        {
            var advert = await _advertRepository.GetAsync(id);

            if (advert == null) throw new NotFoundException("Advert", id);

            await _advertRepository.DeleteAsync(advert);
        }
    }
}
