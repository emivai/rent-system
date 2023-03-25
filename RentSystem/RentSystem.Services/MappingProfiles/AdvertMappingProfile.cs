using AutoMapper;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;

namespace RentSystem.Services.MappingProfiles
{
    public class AdvertMappingProfile : Profile
    {
        public AdvertMappingProfile()
        {
            CreateMap<Advert, GetAdvertDTO>().ReverseMap();
            CreateMap<Advert, AdvertDTO>().ReverseMap();
        }
    }
}
