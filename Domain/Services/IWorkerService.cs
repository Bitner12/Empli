using Domain.Entities;
using Shared.Contracts;
using Shared.Response;

namespace Domain.Services
{
    public interface IWorkerService
    {
        Task<Worker> CreatWorker(WorkerRequest worker);
        Task<int> DeleteWorker(int id);
        Task<List<Worker>> GetAllAsync();
        Task<Worker> GetById(int id);
        Task<List<WorkerDto>> GetWorkerResults(string searchName, DateTime? startDate, DateTime? endDate);
        Task<int> UpdateWorker(int id, string name, decimal costPerHour);
    }
}