using Domain.Entities;
using Shared.Contracts;
using Shared.Response;

namespace Application.Abstratctions
{
    public interface IWorkerService
    {
        Task<Worker> CreatWorker(WorkerRequest workerRequest , Guid companyId);
        Task<Guid> DeleteWorker(Guid id);
        Task<List<Worker>> GetAllAsync(Guid companyId);
        Task<Worker> GetById(Guid id);
        Task<List<WorkerDto>> GetWorkerStatistics(string searchName, DateTime? startDate, DateTime? endDate);
        Task<Worker> UpdateWorker(Guid id, string firstName, string lastName,  decimal costPerHour);
    }
}