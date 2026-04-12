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
        public WorkerRepository(AppDbContext appDbContext)
        {
            _appDbcontext = appDbContext;
        }
        
        
        
         public async Task<Worker> Create(Worker worker)
         {
                    
             await _appDbcontext.Workers.AddAsync(worker);
             await _appDbcontext.SaveChangesAsync();
             return worker;
                    
         }
         
         
         public async Task<Worker> GetById(Guid id)
         {
             return await _appDbcontext.Workers
                 .AsNoTracking()
                 .Include(w => w.Hours)
                 .FirstOrDefaultAsync(w => w.Id == id);
                
         }
         
         
         
         public async Task<List<Worker>> GetAllAsync(Guid companyId)
         {
             return await _appDbcontext.Workers
                 .AsNoTracking()
                 .Where(w => w.CompanyId == companyId)
                 .ToListAsync();
         }
         
         
         
         public async Task<Worker> Update(Worker worker)
         {
              _appDbcontext.Workers.Update(worker);
              await _appDbcontext.SaveChangesAsync();
             return worker;
             
         }
        
         
        
        
        
        public async Task<List<Worker>> GetWorkersWithHours(string search, DateTime? startDate, DateTime? endDate)
        {
            
            
                var query = _appDbcontext.Workers
                    .AsNoTracking()
                    .AsQueryable();

                // 🔎 Поиск по имени и фамилии
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();

                    query = query.Where(w =>
                        w.FirstName.Contains(search) ||
                        w.LastName.Contains(search));
                }

                // 📅 Фильтр часов по датам
                query = query.Include(w => w.Hours
                    .Where(h =>
                        (!startDate.HasValue || h.Date >= startDate.Value) &&
                        (!endDate.HasValue || h.Date <= endDate.Value)));
                if (query == null)
                {
                    return null;
                }

                return await query.ToListAsync();
            
    
        }
        
        
        
        public async Task<Guid> Delete(Guid id)
        {
            await _appDbcontext.Workers
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync();
            return id;
        }
        
       
    }
}
