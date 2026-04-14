using Application.Abstratctions;
using Domain.Entities;
using Infrastructures.Repositories;
using Microsoft.AspNetCore.Identity;
using Shared.Response;

namespace Application.Services;

public class RefreshService : IRefreshService
{
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<User> _userManager;

    public RefreshService(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)

    {
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _userManager = userManager;
    }


    public async Task<LoginResponse> Refresh(string token)
    {
        var refreshToken = await _refreshTokenRepository.GetRefreshToken(token);
        if (refreshToken is null || refreshToken.Expires < DateTime.UtcNow)
        {
            return null;
        }

        await _refreshTokenRepository.DeleteRefreshToken(refreshToken.UserId);

        var user = await _userManager.FindByIdAsync(refreshToken.UserId);
        if (user is null)
        {
            return null;
        }

        var accsess = _tokenService.GenerateToken(user);
        var refresh = _tokenService.GenerateRefreshToken(user);

        await _refreshTokenRepository.AddRefreshToken(refresh);


        return (new LoginResponse(user.Id, user.Email, refresh.Token, accsess));

    }


}