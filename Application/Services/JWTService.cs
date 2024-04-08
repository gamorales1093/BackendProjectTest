using Application.Interfaces;
using Commons.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class JWTService : IJWTService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public JWTService(SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<UserToken> BuildToken(UserLogin userLogin)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(userLogin.Email);
            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.UniqueName, userLogin.Email),
                new(ClaimTypes.Name, user.Email),
                new(JwtRegisteredClaimNames.NameId, user.Id)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de 1 mes.
            var expiration = DateTime.UtcNow.AddMonths(1);

            JwtSecurityToken token = new(
               _configuration["JwtIssuer"],
               _configuration["JwtAudience"],
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        public JwtSecurityToken DecodeJwt(string jwt)
        {
            var key = Encoding.ASCII.GetBytes(this._configuration.GetSection("JwtSecurityKey").Value);
            var handler = new JwtSecurityTokenHandler();

            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            handler.ValidateToken(jwt, validations, out var tokenSecure);

            return (JwtSecurityToken)tokenSecure;
        }
    }
}