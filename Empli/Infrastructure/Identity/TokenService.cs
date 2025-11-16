using Empli.Domian;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Empli.Infrastructure.Identity
{
    public class TokenService(IOptions<AuthSettings> options) : ITokenService
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId",user.Id),
                new Claim("userName",user.UserName),
            };

            var jwtToken = new JwtSecurityToken(
               expires: DateTime.UtcNow.AddMinutes(1),
               claims: claims,
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key)), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);

        }
        public RefreshToken GenerateRefreshToken(User user)
        {
            var token = RandomNumberGenerator.GetBytes(64);

            var expires = DateTime.UtcNow.AddDays(7);
            

            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(token),
                Expires = expires
            };

            return (refreshToken);
        }
    }
}
