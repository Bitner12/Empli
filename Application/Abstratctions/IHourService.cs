using Domain.Entities;
using Shared.Contracts;

namespace Application.Abstratctions
{
    public interface IHourService
    {
        Task<Hour> CreateHour(HourRequest hour);
        Task<List<Hour>> GetAllHours(Guid workerId);
        Task<List<Hour>> GetHours(Guid workerId, DateTime startDate, DateTime endDate, Guid? contractorId = null);
        Task<Guid> DeleteHour(Guid id, DateTime date);
        Task<Guid> UpdateHour(Guid id, float hours, DateTime date, Guid? contractorId, string? comment);
        Task<List<Hour>> GetHoursByContractor(Guid contractorId, DateTime? startDate, DateTime? endDate);
    }
}