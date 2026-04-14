using Application.Abstratctions;
using Domain.Entities;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Identity;
using Shared.Response;

namespace Application.Services
{
    public class LoginService : ILoginService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginService(ITokenService tokenService, UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginResponse> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            var chekPassword = await _userManager.CheckPasswordAsync(user, password);
            if (chekPassword == false)
            {
                return null;
            }
            var accsess = _tokenService.GenerateToken(user);
            var refresh =  _tokenService.GenerateRefreshToken(user);
            
            await _refreshTokenRepository.AddRefreshToken(refresh);
            

            return new LoginResponse(user.Id, user.Email, refresh.Token, accsess);

        }

        public async Task<string> Logout(string userId)
        {
            await _refreshTokenRepository.DeleteRefreshToken(userId);
            return userId;
        }
    }
}
