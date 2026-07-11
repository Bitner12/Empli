using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Infrastructures.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;


namespace Infrastructures.Repositories
{
    public class HourRepository : IHourRepository
    {
        private readonly AppDbContext _appDbcontext;
        public HourRepository(AppDbContext appDbContex)
        {
            _appDbcontext = appDbContex;
        }

        /// <summary>
        /// Npgsql maps to <c>timestamptz</c>; only UTC (or Local converted to UTC) is accepted.
        /// </summary>
        private static DateTime AsUtc(DateTime dt) => dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc),
        };

        public async Task<Hour> Create(HourRequest hour)
        {
            var entity = new Hour
            {
                Date = AsUtc(hour.Date),
                Hours = hour.Hours,
                WorkerId = hour.WorkerId,
                ContractorId = hour.ContractorId,
                Comment = hour.Comment,
            };

            await _appDbcontext.Hours.AddAsync(entity);
            await _appDbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Hour>> Get(Guid workerId)
        {
            return await _appDbcontext.Hours
                .Include(h => h.Contractor)
                .Where(h => h.WorkerId == workerId)
                .ToListAsync();
        }

        public async Task<List<Hour>> GetByDate(Guid workerId, DateTime startDate, DateTime endDate, Guid? contractorId = null)
        {
            var startDay = AsUtc(startDate).Date;
            var endDayExclusive = AsUtc(endDate).Date.AddDays(1);
            var query = _appDbcontext.Hours
                .Include(h => h.Contractor)
                .Where(h => h.WorkerId == workerId)
                .Where(h => h.Date >= startDay && h.Date < endDayExclusive);

            if (contractorId.HasValue)
                query = query.Where(h => h.ContractorId == contractorId.Value);

            return await query.ToListAsync();
        }

        public async Task<Guid> Update(Guid id, float hours, DateTime date, Guid? contractorId, string? comment)
        {
            var d = AsUtc(date);
            await _appDbcontext.Hours
                .Where(h => h.WorkerId == id && h.Date == d)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(h => h.Hours, hours)
                    .SetProperty(h => h.ContractorId, contractorId)
                    .SetProperty(h => h.Comment, comment));

            return id;
        }
        
        public async Task<Guid> Delete(Guid id, DateTime date)
        {
            var d = AsUtc(date);
            await _appDbcontext.Hours
                .Where(h => h.WorkerId == id && h.Date == d)
                .ExecuteDeleteAsync();
            return id;
        }

        public async Task<bool> ExistsForWorkerCalendarDayAsync(Guid workerId, DateTime date)
        {
            var day = AsUtc(date).Date;
            var next = day.AddDays(1);
            return await _appDbcontext.Hours.AnyAsync(
                h => h.WorkerId == workerId && h.Date >= day && h.Date < next);
        }

        public async Task<List<Hour>> GetByContractor(Guid contractorId, DateTime? startDate, DateTime? endDate)
        {
            var query = _appDbcontext.Hours
                .Include(h => h.Worker)
                .Include(h => h.Contractor)
                .Where(h => h.ContractorId == contractorId);

            if (startDate.HasValue)
            {
                var start = AsUtc(startDate.Value).Date;
                query = query.Where(h => h.Date >= start);
            }
            if (endDate.HasValue)
            {
                var endExclusive = AsUtc(endDate.Value).Date.AddDays(1);
                query = query.Where(h => h.Date < endExclusive);
            }

            return await query.OrderBy(h => h.Date).ToListAsync();
        }
    }
}
