using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;

namespace Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    public  CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Company> CreateCompany(string name,string nip,string userId,User user)
    {
      
        var company = new Company()
        {
            
            UserId = user.Id,
            Name = name,
            Nip = nip,
            User = user

        };
        
        return await _companyRepository.Create(company);
    }

    public async Task<Company> GetCompany(string userId)
    {
        return await _companyRepository.Get(userId);
    }

    public async Task<Company> UpdateCompany(string userId, string name ,string nip)
    {
        var company = await GetCompany(userId);
      
        if (company == null)
        {
            return null;
        }

        company.Name = name;
        company.Nip = nip;
        
        return await _companyRepository.Update(company);
        
    }

    public async Task<Guid> DeleteCompany(Guid companyId)
    {
        return await _companyRepository.Delete(companyId);
    }
    
    public async Task <Company> GetByNip (string nip)
    {
        return await _companyRepository.ByNip(nip);
    }
}