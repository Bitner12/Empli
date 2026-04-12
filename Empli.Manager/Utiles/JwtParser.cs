using System.Security.Claims;

namespace Empli.Manager.Utiles;


public static class JwtParser
{
    public static string GetUserIdFromHttpContext(HttpContext context)
    {
      
        return context.User.Claims.FirstOrDefault(x => x.Type.Equals("userId", StringComparison.OrdinalIgnoreCase))?.Value   
            ;

      
    }
}