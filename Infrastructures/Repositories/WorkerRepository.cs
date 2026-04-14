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

        private static DateTime AsUtc(DateTime dt) => dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc),
        };

        
        
         public async Task<Worker> Create(Worker worker)
         {
                    
             await _appDbcontext.Workers.AddAsync(worker);
             await _appDbcontext.SaveChangesAsync();
             return worker;
                    
         }

        public async Task<Worker> GetWorkerByPesel(string pesel)
        {
            return await _appDbcontext.Workers
                .Include(w => w.Hours)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Pesel == pesel)
                                                             ;

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
                 .Include(w => w.Hours)
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

                // 📅 Фильтр часов по датам, конец периода включительно
                var startDay = startDate.HasValue ? AsUtc(startDate.Value).Date : (DateTime?)null;
                var endDayExclusive = endDate.HasValue ? AsUtc(endDate.Value).Date.AddDays(1) : (DateTime?)null;
                query = query.Include(w => w.Hours
                    .Where(h =>
                        (!startDay.HasValue || h.Date >= startDay.Value) &&
                        (!endDayExclusive.HasValue || h.Date < endDayExclusive.Value)));
                if (query == null)
                {
                    return null;
                }

                return await query.ToListAsync();
            
    
        }
        
        
        
        public async Task UnlinkFromCompanyAsync(Guid workerId)
        {
            await _appDbcontext.Workers
                .Where(w => w.Id == workerId)
                .ExecuteUpdateAsync(s => s.SetProperty(w => w.CompanyId, (Guid?)null));
        }

        public async Task<Guid> DeleteWorkerAndHoursAsync(Guid workerId)
        {
            await _appDbcontext.Hours.Where(h => h.WorkerId == workerId).ExecuteDeleteAsync();
            await _appDbcontext.Workers.Where(w => w.Id == workerId).ExecuteDeleteAsync();
            return workerId;
        }

        public async Task<Guid> Delete(Guid id)
        {
            return await DeleteWorkerAndHoursAsync(id);
        }
        
       
    }
}
