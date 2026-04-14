

using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Infrastructures.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _appDbContext;
        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Employee> Create(Employee employee)
        {
            await _appDbContext.Employees.AddAsync(employee);
            await _appDbContext.SaveChangesAsync();
            return employee;

        }

        public async Task<Employee> Get(string userId)
        {
            return await _appDbContext.Employees
                 .Include(e => e.Worker)
                 .Where(e => e.UserId == userId)
                 .FirstOrDefaultAsync();

        }

        public async Task<Guid> Delete(Guid id)
        {

            await _appDbContext.Employees
                .Where(e => e.Id == id)
                .ExecuteDeleteAsync();
            return id;

        }
        public async Task<Employee> Update(Employee employee)
        {
            _appDbContext.Employees.Update(employee);
            await _appDbContext.SaveChangesAsync();
            return employee;

        }

        public async Task<bool> AnyEmployeeForWorkerIdAsync(Guid workerId)
        {
            return await _appDbContext.Employees.AnyAsync(e => e.WorkerId == workerId);
        }


    }
}
