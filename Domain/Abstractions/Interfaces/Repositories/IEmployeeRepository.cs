using Domain.Entities;

namespace Domain.Abstractions.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> Create(Employee employee);
        Task<Guid> Delete(Guid id);
        Task<Employee> Get(string userId);
        Task<Employee> Update(Employee employee);
        Task<bool> AnyEmployeeForWorkerIdAsync(Guid workerId);
    }
}