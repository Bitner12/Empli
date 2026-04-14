using Domain.Entities;
using Shared.Contracts;


namespace Domain.Abstractions.Interfaces.Repositories
{
    public interface IWorkerRepository
    {
        Task <Worker> Create(Worker worker);
        Task<Guid> Delete(Guid id);
        Task UnlinkFromCompanyAsync(Guid workerId);
        Task<Guid> DeleteWorkerAndHoursAsync(Guid workerId);
        Task <Worker> GetWorkerByPesel (string  pesel);
        Task<List<Worker>> GetAllAsync(Guid companyId);
        Task<Worker> GetById(Guid id);
        Task <List<Worker>> GetWorkersWithHours(string search, DateTime? startDate, DateTime? endDate);
        Task <Worker> Update(Worker worker);
    }
}