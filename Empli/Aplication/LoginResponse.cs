using Empli.Infrastructure.Identity;

namespace Empli.Aplication
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public RefreshToken RefreshToken { get; set; }

    }
}
