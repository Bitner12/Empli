using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;


namespace Application.Services
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


        public async Task<List<Hour>> GetAllHours(Guid workerId)
        {
            return await _hourRepository.Get(workerId);
        }

        public async Task<List<Hour>> GetHours(Guid workerId, DateTime startDate, DateTime endDate)
        {
            return await _hourRepository.GetByDate(workerId, startDate, endDate);
        }
        
        
        public async Task<Guid> UpdateHour(Guid id, float hours, DateTime date)
        {
            await _hourRepository.Update(id, hours, date);
            return id;

        }
        
        public async Task<Guid> DeleteHour(Guid id, DateTime date) 
        {
            await _hourRepository.Delete(id, date);
            return id;
        }
        
    }
}
