using AutoMapper;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;

namespace RentSystem.Services.MappingProfiles
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile() 
        {
            CreateMap<Item, GetItemDTO>()
                .ForMember(m => m.Category,
                       opt => opt.MapFrom(o => o.Category))
                .ForMember(m => m.State,
                       opt => opt.MapFrom(o => o.State))
                .ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();
        }
    }
}
