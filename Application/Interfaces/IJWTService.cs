using Commons.Classes;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Interfaces
{
    public interface IJWTService
    {
        Task<UserToken> BuildToken(UserLogin userLogin);
        JwtSecurityToken DecodeJwt(string jwt);
    }
}
