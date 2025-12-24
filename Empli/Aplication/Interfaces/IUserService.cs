using Empli.Domain;
using Microsoft.AspNetCore.Identity;

namespace Empli.Aplication.Interfaces
{
    public interface IUserService
    {
        Task<(User, IdentityResult)> CreateUser(global::System.String email, global::System.String password);
        Task<User> GetUser(global::System.String email, string password);
        Task<User> GetUserById(global::System.String id);
    }
}