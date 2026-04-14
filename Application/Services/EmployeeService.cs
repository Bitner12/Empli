
using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWorkerService _workerService;
        private readonly ICompanyService _companyService;
        public EmployeeService(IEmployeeRepository employeeRepository ,IWorkerService workerService, ICompanyService companyService )
        {
            _companyService = companyService;
            _workerService = workerService;
            _employeeRepository = employeeRepository;
        }
        public async Task<Employee> EmployeeCreate(string firstName, string lastName, decimal? costPerHour, User user, string pesel)
        {
            var worker = await _workerService.CreatWorker(pesel, firstName, lastName, costPerHour );
            var employee = new Employee()
            {
                Pesel = pesel,
                FirstName = firstName,
                LastName = lastName,
                UserId = user.Id,         
                WorkerId = worker.Id,

            };
           

            await _employeeRepository.Create(employee);
          

            return employee;
        }


        


        public async Task<Employee> EmployeeGet(string userId)
        {

            return await _employeeRepository.Get(userId);

        }


        public async Task<Employee?> EmployeeUpdateProfile(string userId, string firstName, string lastName, decimal? costPerHour)
        {
            var employee = await _employeeRepository.Get(userId);
            if (employee == null)
            {
                return null;
            }

            if (employee.Worker?.CompanyId != null)
            {
                return null;
            }

            employee.FirstName = firstName;
            employee.LastName = lastName;
            await _employeeRepository.Update(employee);

            if (employee.WorkerId.HasValue)
            {
                var rate = costPerHour ?? employee.Worker?.CostPerHour ?? 0m;
                await _workerService.UpdateWorker(employee.WorkerId.Value, firstName, lastName, rate);
            }

            return await _employeeRepository.Get(userId);
        }


        public async Task<Guid> EmployeeDelete(Guid employeeId)


        {

            return await _employeeRepository.Delete(employeeId);
        }






    }
}
