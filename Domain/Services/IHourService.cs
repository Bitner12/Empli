using Domain.Entities;
using Shared.Contracts;

namespace Domain.Services
{
    public interface IHourService
    {
        Task<Hour> CreateHour(HourRequest hour);
        Task<int> DeleteHour(int id, DateTime date);
        Task<int> UpdateHour(int id, float hours, DateTime date);
    }
}