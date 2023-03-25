using AutoMapper;
using RentSystem.Core.Contracts.Repository;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;

namespace RentSystem.Services.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly IAdvertRepository _advertRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public AdvertService(IAdvertRepository advertRepository, IItemRepository itemRepository, IMapper mapper)
        {
            _advertRepository = advertRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetAdvertDTO>> GetAllAsync()
        {
            var adverts = await _advertRepository.GetAllAsync();

            return _mapper.Map<List<GetAdvertDTO>>(adverts);
        }

        public async Task<GetAdvertDTO> GetAsync(int id)
        {
            var advert = await _advertRepository.GetAsync(id);

            if (advert == null)
            {
                throw new Exception();
            }

            return _mapper.Map<GetAdvertDTO>(advert);
        }

        public async Task CreateAsync(AdvertDTO advertDTO)
        {
            var advert = _mapper.Map<Advert>(advertDTO);

            await _advertRepository.CreateAsync(advert);
        }

        public async Task UpdateAsync(int id, AdvertDTO advertDTO)
        {
            var advert = await _advertRepository.GetAsync(id);

            if (advert == null)
            {
                throw new Exception();
            }

            advert.Description = advertDTO.Description;
            advert.ImageUrl = advertDTO.ImageUrl;
            advert.VideoUrl = advertDTO.VideoUrl;
            advert.DeliveryType = advertDTO.DeliveryType;
            advert.RentStart = advertDTO.RentStart;
            advert.RentEnd = advertDTO.RentEnd;

            await _advertRepository.UpdateAsync(advert);
        }

        public async Task DeleteAsync(int id)
        {
            var advert = await _advertRepository.GetAsync(id);

            if (advert == null)
            {
                throw new Exception();
            }

            await _advertRepository.DeleteAsync(advert);
        }
    }
}
