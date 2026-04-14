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
        private readonly IEmployeeRepository _employeeRepository;

        public WorkerService(IWorkerRepository workerRepository, IEmployeeRepository employeeRepository)
        {
            _workerRepository = workerRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<Worker> CreatWorker(string pesel,string firstName, string lastName, decimal? costPerHour)
        {
            var checkWorker =  await _workerRepository.GetWorkerByPesel(pesel);
            if ( checkWorker == null)
            {
               var workerEntity =  new Worker()
               {
                   
                   Pesel = pesel,
                   FirstName = firstName,
                   LastName = lastName,
                   CostPerHour = costPerHour,
               };
                return await _workerRepository.Create(workerEntity);
            }
            
            return checkWorker;
        }

      


        public async Task<Worker> GetByPesel(string pesel)
        {

            return await _workerRepository.GetWorkerByPesel(pesel);
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
            if (await _employeeRepository.AnyEmployeeForWorkerIdAsync(id))
            {
                await _workerRepository.UnlinkFromCompanyAsync(id);
                return id;
            }

            return await _workerRepository.DeleteWorkerAndHoursAsync(id);
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
                TotalHours = w.Hours.Sum(h => h.Hours),
                TotalCost = (decimal)((decimal)w.Hours.Sum(h => h.Hours) * w.CostPerHour)
            }).ToList();

            
        }


    }
}
