using Domain.Entities;
using Domain.Enums;

namespace Application.Abstratctions;

public interface IUserService
{
    Task<User> GetUser(string userId);
    Task<User> UpdateUser(User user, UserType userType, Guid userTypeId, Company? company, Employee? employee);
    Task<string> DeleteUser(string userId);
}