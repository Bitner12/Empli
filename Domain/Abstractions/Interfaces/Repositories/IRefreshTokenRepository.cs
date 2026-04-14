using Domain.Entities;

namespace Infrastructures.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetRefreshToken(string refreshToken);
    Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken);
    Task<int> DeleteRefreshToken(string userId);
}