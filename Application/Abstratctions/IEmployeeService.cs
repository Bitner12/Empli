using Domain.Entities;

namespace Application.Services
{
    public interface IEmployeeService
    {
        Task<Employee> EmployeeCreate(string firstName, string lastName, decimal? costPerHour, User user, string pesel);
        Task<Guid> EmployeeDelete(Guid employeeId);
        Task<Employee> EmployeeGet(string userId);
        Task<Employee?> EmployeeUpdateProfile(string userId, string firstName, string lastName, decimal? costPerHour);
    }
}