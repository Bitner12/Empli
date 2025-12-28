using Empli.Aplication.Models;
using Empli.Domain;
using Microsoft.AspNetCore.Identity;

namespace Empli.Aplication.Interfaces
{
    public interface IUserService
    {
        Task<(User, IdentityResult)> CreateUser(global::System.String email, global::System.String password);
        Task<User> GetUser(global::System.String id);
        Task<User?> Login (global::System.String id, string password);
        Task<User> RefreshUser(string id, string refresh);
    }
}