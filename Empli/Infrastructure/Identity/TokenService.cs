using Empli.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Empli.Infrastructure.Identity
{
    public class TokenService : ITokenService
    {
        private readonly byte[] _key;
        public TokenService(IOptions<AuthSettings> options)
        {
            _key = Encoding.UTF8.GetBytes(options.Value.Key);
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId",user.Id),
                new Claim("userName",user.UserName),
            };

            var jwtToken = Create(claims);

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

            return refreshToken;
        }

        private JwtSecurityToken Create(IEnumerable<Claim> claims) 
        {
            return new JwtSecurityToken(
               expires: DateTime.UtcNow.AddMinutes(1),
               claims: claims,
               signingCredentials: new SigningCredentials(
                   new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256));
        }
    }
}
