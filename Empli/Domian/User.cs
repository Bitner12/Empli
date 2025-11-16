using Microsoft.AspNetCore.Identity;

namespace Empli.Domian
{
    public class User : IdentityUser
    {
       public string RefreshToken { get; set; }
       public DateTime Expires { get; set; }
    }
}
