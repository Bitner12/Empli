using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Infrastructures.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories;



public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _appDbContext;
    public CompanyRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    public async  Task<Company> Create(Company company)
    {
        await _appDbContext.Companies.AddAsync(company);
        await _appDbContext.SaveChangesAsync();
        return company;
    }


    public async Task<Company> Get(string userId)
    {
        return await _appDbContext.Companies
            .Where(c => c.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<Company> Update(Company company)
    {
        _appDbContext.Companies.Update(company);
        await _appDbContext.SaveChangesAsync();
        return company;
    }

    public async Task<Guid> Delete(Guid companyId)
    {

        await _appDbContext.Companies
            .Where(c => c.Id == companyId)
            .ExecuteDeleteAsync();
        return companyId;
    }

    public async Task<Company> ByNip(string nip)
    {
        return await _appDbContext.Companies
            .Where(c => c.Nip == nip)
            .FirstOrDefaultAsync();
    }
    
}