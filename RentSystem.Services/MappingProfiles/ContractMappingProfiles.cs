using AutoMapper;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;

namespace RentSystem.Services.MappingProfiles
{
    public class ContractMappingProfiles : Profile
    {
        public ContractMappingProfiles() 
        {
            CreateMap<Contract, ContractDTO>().ReverseMap();
            CreateMap<Contract, GetContractDTO>().ReverseMap();
        }
    }
}
