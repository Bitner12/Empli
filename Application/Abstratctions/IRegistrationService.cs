using Domain.Entities;

namespace Application.Abstratctions
{
    public interface IRegistrationService
    {
        Task<User> Registration(string email, string password);
    }
}