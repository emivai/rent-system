using AutoMapper;
using RentSystem.Core.DTOs;
using RentSystem.Core.Entities;
using RentSystem.Core.Extensions;

namespace RentSystem.Services.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterUserDTO, User>()
                .AfterMap((src, dest) => dest = Resolver(dest));

            CreateMap<User, UserDTO>().ReverseMap();
        }

        public static User Resolver(User dest) 
        {
            var hashValue = dest.Password.Hash();

            dest.Password = hashValue.Hash;
            dest.Salt = hashValue.Salt;

            return dest;
        }
    }
}
