using Empli.Domain;

namespace Empli.Infrastructure.Identity
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        RefreshToken GenerateRefreshToken(User user);
    }
}