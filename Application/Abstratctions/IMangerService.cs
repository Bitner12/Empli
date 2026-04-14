using Domain.Entities;
using Shared.Contracts;
using Shared.Response;

namespace Application.Abstratctions
{
    public interface IMangerService
    {
        Task<Worker> AddWorker(WorkerRequest workerRequest, string userId);
        Task<List<Worker>> GetWorkers(string userId);
        Task<WorkerDto> GetWorkerByPesel(string pesel, string userId);
        Task<Worker> UpdateWorker(Guid id, string firstName, string lastName, decimal costPerHour, string userId);
        Task<bool> DeleteWorker(Guid id, string userId);
    }
}