using Domain.Entities;
using Shared.Contracts;


namespace Domain.Abstractions.Interfaces.Repositories
{
    public interface IWorkerRepository
    {
        Task <Worker> Create(WorkerRequest worker);
        Task <int> Delete(int id);
        Task<List<Worker>> GetAllAsync();
        Task<Worker> GetById(int id);
        Task <List<Worker>> GetWorkersWithHours(string search, DateTime? startDate, DateTime? endDate);
        Task <int> Update(int id, string name, decimal costPerHour);
    }
}