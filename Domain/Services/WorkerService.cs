using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;
using Shared.Response;

namespace Domain.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;

        public WorkerService(IWorkerRepository workerRepository)
        {
            _workerRepository = workerRepository;
        }

        public async Task<Worker> CreatWorker(WorkerRequest worker)
        {
            return await _workerRepository.Create(worker);
        }
        public async Task<int> DeleteWorker(int id)
        {
            return await _workerRepository.Delete(id);
        }
        public async Task<int> UpdateWorker(int id, string name, decimal costPerHour)
        {
           
            return await _workerRepository.Update(id, name, costPerHour);
        }
        public async Task<List<Worker>> GetAllAsync()
        {
            return await _workerRepository.GetAllAsync();
        }
        public async Task<Worker> GetById(int id)
        {
            return await _workerRepository.GetById(id);
        }
        public async Task<List<WorkerDto>> GetWorkerResults(string searchName, DateTime? startDate, DateTime? endDate)//TODO: Погана назва метода.
        {
            var workers = await _workerRepository.GetWorkersWithHours(searchName, startDate, endDate);
            //TODO: Змінна result тут не потрібна, можна повернути результат прямо з Select.
            var result = workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                Name = w.Name,
                CostPerHour = w.CostPerHour,
                Hours = w.Hours.Select(h => new HourDto
                {
                    Id = h.Id,
                    Date = h.Date.ToString("dd-MM-yyyy"),
                    Hours = (decimal)h.Hours
                }).ToList(),
                TotalHours = (decimal)w.Hours.Sum(h => h.Hours),
                TotalCost = (decimal)w.Hours.Sum(h => h.Hours) * w.CostPerHour
            }).ToList();

            return result; //TODO: А що буде коли юзера не буде знайдено?
        }


    }
}
