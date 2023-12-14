using RentSystem.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace RentSystem.Core.Contracts.Service
{
    public interface ITokenManager
    {
        string CreateAccessTokenAsync(User user);
        JwtSecurityToken? DecodeAccessTokenAsync(string token);
    }
}
