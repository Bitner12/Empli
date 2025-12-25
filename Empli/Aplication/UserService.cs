using Empli.Aplication.Interfaces;
using Empli.Aplication.Models;
using Empli.Domain;
using Empli.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

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


        public async Task<User> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<Result<User>> Login(string email ,string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<User>.Failure("User not found");
            }

            var chekPassword = await _userManager.CheckPasswordAsync(user, password);
            if(chekPassword == false)
            {
                return Result<User>.Failure("Неверный пароль");
            }

            return Result<User>.Success(user);

        }
        public async Task<Result<User>> RefreshUser (string id , string refresh)
        {
            var user = await GetUser(id);
            if (user == null)
            {
                return Result<User>.Failure("User not found");
            }
            if (user.Expires < DateTime.UtcNow || user.RefreshToken != refresh)
            {
                return Result<User>.Failure("Token is invalid");
            }
            var newRefresh = _tokenService.GenerateRefreshToken(user);
            user.RefreshToken = newRefresh.Token;
            user.Expires = newRefresh.Expires;
            await _userManager.UpdateAsync(user);
            return Result<User>.Success(user);
        }
    }
}
