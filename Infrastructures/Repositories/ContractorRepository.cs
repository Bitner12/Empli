using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Infrastructures.Contexts;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace Infrastructures.Repositories
{
    public class ContractorRepository : IContractorRepository
    {
        private readonly AppDbContext _db;

        public ContractorRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Contractor> Create(Guid companyId, ContractorRequest request)
        {
            var contractor = new Contractor
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                CompanyId = companyId,
                RatePerHour = request.RatePerHour,
            };
            _db.Contractors.Add(contractor);
            await _db.SaveChangesAsync();
            return contractor;
        }

        public async Task<Contractor?> Update(Guid id, Guid companyId, ContractorRequest request)
        {
            var contractor = await _db.Contractors
                .FirstOrDefaultAsync(c => c.Id == id && c.CompanyId == companyId);
            if (contractor is null) return null;
            contractor.Name = request.Name;
            contractor.RatePerHour = request.RatePerHour;
            await _db.SaveChangesAsync();
            return contractor;
        }

        public async Task<List<Contractor>> GetByCompany(Guid companyId)
            => await _db.Contractors
                .Where(c => c.CompanyId == companyId)
                .OrderBy(c => c.Name)
                .ToListAsync();

        public async Task<List<Contractor>> GetByWorker(Guid workerId)
        {
            var companyId = await _db.Workers
                .Where(w => w.Id == workerId)
                .Select(w => w.CompanyId)
                .FirstOrDefaultAsync();

            if (companyId == null) return [];

            return await _db.Contractors
                .Where(c => c.CompanyId == companyId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task Delete(Guid id, Guid companyId)
        {
            await _db.Contractors
                .Where(c => c.Id == id && c.CompanyId == companyId)
                .ExecuteDeleteAsync();
        }
    }
}