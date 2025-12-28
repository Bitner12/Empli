using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;


namespace Domain.Services
{
    public class HourService : IHourService
    {
        private readonly IHourRepository _hourRepository;
        public HourService(IHourRepository hourRepository)
        {
            _hourRepository = hourRepository;
        }

        public async Task<Hour> CreateHour(HourRequest hour)
        {
            return await _hourRepository.Create(hour);
        }
        public async Task<int> DeleteHour(int id, DateTime date) 
        {
            await _hourRepository.Delete(id, date);
            return id;
        }
        public async Task<int> UpdateHour(int id, float hours, DateTime date)
        {
            await _hourRepository.Update(id, hours, date);
            return id;

        }
    }
}
