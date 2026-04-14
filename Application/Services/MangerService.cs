using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;
using Shared.Response;


namespace Application.Services
{
    public class MangerService : IMangerService
    {
        private readonly IWorkerService _workerService;
        private readonly ICompanyService _companyService;
        private readonly IWorkerRepository _workerRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public MangerService(IWorkerService workerService, ICompanyService companyService, IWorkerRepository workerRepository, IEmployeeRepository employeeRepository)
        {
            _workerRepository = workerRepository;
            _workerService = workerService;
            _companyService = companyService;
            _employeeRepository = employeeRepository;
        }

        public async Task<Worker> AddWorker(WorkerRequest workerRequest, string userId)
        {

            var company = await _companyService.GetCompany(userId);
            if (company == null)
            {
                return null;
            }

            var newWorker = await _workerService.CreatWorker(workerRequest.Pesel, workerRequest.FirstName,workerRequest.LastName, workerRequest.CostPerHour);
            if (newWorker == null)
            {
                return null;
            }
            newWorker.EmployeeId = null;
            newWorker.Employee = null;
            newWorker.CompanyId = company.Id;
            newWorker.Company = company;

            await _workerRepository.Update(newWorker);

            return newWorker;
        }

        public async Task<List<Worker>> GetWorkers(string userId)
        {
            var company = await _companyService.GetCompany(userId);
            if (company == null)
            {
                return null;
            }

            var workers = await _workerRepository.GetAllAsync(company.Id);
            return workers;

        }

        public async Task<WorkerDto> GetWorkerByPesel(string pesel, string userId)
        {
            var company = await _companyService.GetCompany(userId);
            if (company == null)
            {
                return null;
            }
            var worker = await _workerRepository.GetWorkerByPesel(pesel);
            if (worker == null || worker.CompanyId != company.Id)
            {
                return null;
            }
            var workerDto = new WorkerDto
            {
                Id = worker.Id,
                Pesel = worker.Pesel,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                CostPerHour = (decimal)worker.CostPerHour,
                Hours = worker.Hours.Select(h => new HourDto
                {
                    Id = h.Id,
                    Date = h.Date,
                    Hours = h.Hours

                }).ToList() ?? new List<HourDto>(),
                //TotalHours = worker.Hours.Sum(h => h.Hours),
                //TotalCost = worker.Hours.Sum(h => h.Hourswo * worker.CostPerHour)
            };

            return workerDto;
        }

        public async Task<Worker> UpdateWorker(Guid id, string firstName, string lastName, decimal costPerHour, string userId)
        {
            var company = await _companyService.GetCompany(userId);
            if (company == null)
            {
                return null;
            }
            var worker = await _workerRepository.GetById(id);
            if (worker == null || worker.CompanyId != company.Id)
            {
                return null;
            }
            worker.FirstName = firstName;
            worker.LastName = lastName;
            worker.CostPerHour = costPerHour;
            await _workerRepository.Update(worker);
            return worker;

        }
        public async Task<bool> DeleteWorker(Guid id, string userId)
        {
            var company = await _companyService.GetCompany(userId);
            if (company == null)
            {
                return false;
            }
            var worker = await _workerRepository.GetById(id);
            if (worker == null || worker.CompanyId != company.Id)
            {
                return false;
            }

            if (await _employeeRepository.AnyEmployeeForWorkerIdAsync(worker.Id))
            {
                await _workerRepository.UnlinkFromCompanyAsync(worker.Id);
            }
            else
            {
                await _workerRepository.DeleteWorkerAndHoursAsync(worker.Id);
            }

            return true;
        }
    }
}
