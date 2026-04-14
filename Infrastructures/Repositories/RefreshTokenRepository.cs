using Domain.Entities;
using Infrastructures.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _appDbContext;

    public RefreshTokenRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<RefreshToken> GetRefreshToken(string token)
    {
        return await _appDbContext.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        await _appDbContext.RefreshTokens
            .Where(rt => rt.UserId == refreshToken.UserId)
            .ExecuteDeleteAsync();

        _appDbContext.RefreshTokens.Add(refreshToken);
        await _appDbContext.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<int> DeleteRefreshToken(string userId)
    {
        return await _appDbContext.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ExecuteDeleteAsync();
    }
}
