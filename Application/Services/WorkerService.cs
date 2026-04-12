using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;
using Shared.Response;

namespace Application.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;

        public WorkerService(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<Worker> CreatWorker(WorkerRequest workerRequest,  Guid companyId)
        {
            var workerEntity = new Worker()
            {
                CompanyId = companyId,
                FirstName = workerRequest.FirstName,
                LastName = workerRequest.LastName,
                CostPerHour = workerRequest.CostPerHour
            
            };
            
            return await _workerRepository.Create(workerEntity);
        }

        public async Task<Worker> GetById(Guid id)
        {
            return await _workerRepository.GetById(id);

        }
        
        
        public async Task<List<Worker>> GetAllAsync(Guid companyId)
        {
            return await _workerRepository.GetAllAsync(companyId);
        }
        
        
        public async Task<Worker> UpdateWorker(Guid id, string firstName,string lastName, decimal costPerHour)
        {
            var worker = await GetById(id);
            worker.FirstName = firstName;
            worker.LastName = lastName;
            worker.CostPerHour = costPerHour;   
            
            
            return await _workerRepository.Update(worker);
        }
        

        public async Task<Guid> DeleteWorker(Guid id)
        {
            return await _workerRepository.Delete(id);
        }
        
        
       
        
        public async Task<List<WorkerDto>> GetWorkerStatistics(string searchName, DateTime? startDate, DateTime? endDate)
        {
            var workers = await _workerRepository.GetWorkersWithHours(searchName, startDate, endDate);
            if (workers.Count() == 0)
            {
                return null;
            }
            
            return workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                FirstName = w.FirstName,
                LastName = w.LastName,
                CostPerHour = w.CostPerHour,
                Hours = w.Hours.Select(h => new HourDto
                {
                    Id = h.Id,
                    Date = h.Date,
                    Hours = h.Hours
                }).ToList(),
                TotalHours = (decimal)w.Hours.Sum(h => h.Hours),
                TotalCost = (decimal)w.Hours.Sum(h => h.Hours) * w.CostPerHour
            }).ToList();

            
        }


    }
}
