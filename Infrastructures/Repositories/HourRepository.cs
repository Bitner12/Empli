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

        public async Task<Hour> Create(HourRequest hour)
        {
            var hourRequest = new Hour()
            {
                Date = hour.Date,
                Hours = hour.Hours,
                WorkerId = hour.WorkerId
            };

            await _appDbcontext.Hours.AddAsync(hourRequest);
            await _appDbcontext.SaveChangesAsync();
            return hourRequest;
        }
        public async Task<int> Delete(int id, DateTime date)
        {
            await _appDbcontext.Hours
                .Where(h => h.WorkerId == id && h.Date == date)
                .ExecuteDeleteAsync();
            return id;
        }
        public async Task<int> Update(int id, float hours, DateTime date)
        {
            await _appDbcontext.Hours
                .Where(h => h.WorkerId == id && h.Date == date)
                .ExecuteUpdateAsync(s => s
                .SetProperty(h => h.Hours, hours));
                

            return id;
        }
       
    }
}
