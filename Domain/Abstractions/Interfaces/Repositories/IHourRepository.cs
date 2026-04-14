using Domain.Entities;
using Shared.Contracts;

namespace Domain.Abstractions.Interfaces.Repositories
{
    public interface IHourRepository
    {
        Task<Hour> Create(HourRequest hour); 
        Task<List<Hour>> Get(Guid workerId);
        Task<List<Hour>> GetByDate(Guid workerId, DateTime startDate, DateTime endDate);
        Task<Guid> Delete(Guid id, DateTime date);
        Task<Guid> Update(Guid id, float hours, DateTime date);
        Task<bool> ExistsForWorkerCalendarDayAsync(Guid workerId, DateTime date);
    }
}