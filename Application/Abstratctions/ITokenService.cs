using Domain.Entities;

namespace Application.Abstratctions
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        RefreshToken GenerateRefreshToken(User user);
    }
}