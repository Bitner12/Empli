using Empli.Aplication.Interfaces;
using Empli.Domain;
using Empli.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Empli.Aplication
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public UserService(ITokenService tokenService, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<(User?,IdentityResult)> CreateUser(string email, string password)
        {
            var user = new User { UserName = email, Email = email };
            var refreshToken = _tokenService.GenerateRefreshToken(user);
            user.RefreshToken = refreshToken.Token;
            user.Expires = refreshToken.Expires;
            var result = await _userManager.CreateAsync(user, password);
            
            return (result.Succeeded? user : null, result);
        }


        public async Task<User> GetUser(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email.ToLowerInvariant());
            if (user == null)
            {
               throw new UnauthorizedAccessException();
            }
            var checkPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!checkPassword)
            {
                throw new UnauthorizedAccessException();
            }
            
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);
            user.RefreshToken = newRefreshToken.Token;
            user.Expires = newRefreshToken.Expires;
            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task<User> GetUserById(string id)
        {
            return null;
        }
    }
}
