using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;


namespace Application.Services
{
    
    public class HourService : IHourService
    {
        public const string DuplicateCalendarDayMessage = "На эту дату уже есть запись учёта часов.";

        private readonly IHourRepository _hourRepository;
        public HourService(IHourRepository hourRepository)
        {
            _hourRepository = hourRepository;
        }

        private static DateTime CalendarDayUtc(DateTime dt) => dt.Kind switch
        {
            DateTimeKind.Utc => dt.Date,
            DateTimeKind.Local => dt.ToUniversalTime().Date,
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc).Date,
        };

        private static void EnsureHourDateNotAfterToday(DateTime date)
        {
            if (CalendarDayUtc(date) > DateTime.UtcNow.Date)
            {
                throw new InvalidOperationException("Дата записи не может быть позже текущего дня.");
            }
        }

        public async Task<Hour> CreateHour(HourRequest hour)
        {
            EnsureHourDateNotAfterToday(hour.Date);
            if (await _hourRepository.ExistsForWorkerCalendarDayAsync(hour.WorkerId, hour.Date))
            {
                throw new InvalidOperationException(DuplicateCalendarDayMessage);
            }

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
            EnsureHourDateNotAfterToday(date);
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
