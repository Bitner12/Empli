using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Infrastructures.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;


namespace Infrastructures.Repositories
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly AppDbContext _appDbcontext;
        public WorkerRepository(AppDbContext appDbContex)
        {
            _appDbcontext = appDbContex;
        }
        public async Task<List<Worker>> GetAllAsync()
        {
            return await _appDbcontext.Workers
                          .AsNoTracking()
                          .ToListAsync();
        }
        public async Task<List<Worker>> GetWorkersWithHours(string search, DateTime? startDate, DateTime? endDate)
        {
            return await _appDbcontext.Workers

                          .AsNoTracking()
                          .Where(w => w.Name.ToLower().Contains(search.ToLower()))
                          .Include(w => w.Hours.Where(h =>(!startDate.HasValue || h.Date >= startDate.Value) &&
                                                                                               (!endDate.HasValue || h.Date <= endDate.Value)))                        
                          .ToListAsync();
    
        }
        public async Task<Worker> GetById(int id)
        {
            return await _appDbcontext.Workers
                .AsNoTracking()
                .Include(w => w.Hours)
                .FirstOrDefaultAsync(w => w.Id == id);
                
        }
        public async Task<Worker> Create(WorkerRequest worker)
        {
            //TODO: За створення співробітка повинен відповідати сервіс а не репозиторій.
            var workerEntity = new Worker()
            {
                Name = worker.Name,
                CostPerHour = worker.CostPerHour,
               
            };
           

            var result = await _appDbcontext.Workers.AddAsync(workerEntity);
            await _appDbcontext.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<int> Update(int id, string name, decimal costPerHour)
        {
            //TODO: За оновлення співробітка повинен відповідати сервіс а не репозиторій. Оновлення виконуй за допомогую трекінгу. Тобто отримай співробітка з бази даних, онови його властивості і збережи зміни.
            await _appDbcontext.Workers
                .Where(w => w.Id == id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(w => w.Name, name)
                .SetProperty(w => w.CostPerHour, costPerHour));
            return id;
        }
        public async Task<int> Delete(int id)
        {
            await _appDbcontext.Workers
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
    }
}
