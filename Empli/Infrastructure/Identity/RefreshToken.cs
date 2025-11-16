using Microsoft.AspNetCore.Identity;

namespace Empli.Infrastructure.Identity
{
    public class RefreshToken 
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
