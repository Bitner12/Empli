using Domain.Entities;
using Shared.Contracts;

namespace Application.Abstratctions
{
    public interface IHourService
    {
        Task<Hour> CreateHour(HourRequest hour);
        Task<List<Hour>> GetAllHours(Guid workerId);
        Task<List<Hour>> GetHours(Guid workerId,DateTime startDate, DateTime endDate);
        Task<Guid> DeleteHour(Guid id, DateTime date);
        Task<Guid> UpdateHour(Guid id, float hours, DateTime date);
    }
}