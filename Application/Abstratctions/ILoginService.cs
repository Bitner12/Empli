using Shared.Response;

namespace Application.Abstratctions
{
    public interface ILoginService
    {
        Task<LoginResponse> Login(string email, string password);
        Task<string> Logout(string userId);
    }
}