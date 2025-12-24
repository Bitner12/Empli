using Microsoft.AspNetCore.Identity;

namespace Empli.Domain
{
    public class User : IdentityUser
    {
       public string RefreshToken { get; set; }
       public DateTime Expires { get; set; } //This name cant be infromed to ExpiryDate bc of IdentityUser class
    }
}
