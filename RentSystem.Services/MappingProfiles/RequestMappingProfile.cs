using AutoMapper;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;

namespace RentSystem.Services.MappingProfiles
{
    public class RequestMappingProfile : Profile
    {
        public RequestMappingProfile()
        {
            CreateMap<Request, GetRequestDTO>()
                .ForMember(m => m.Category,
                       opt => opt.MapFrom(o => o.Category))
                .ReverseMap();
            CreateMap<Request, RequestDTO>().ReverseMap();
        }
    }
}
