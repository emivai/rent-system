using AutoMapper;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;

namespace RentSystem.Services.MappingProfiles
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile() 
        {
            CreateMap<Item, GetItemDTO>().ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();
        }
    }
}
