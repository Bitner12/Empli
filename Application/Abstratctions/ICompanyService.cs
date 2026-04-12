using Domain.Entities;
using Shared.Contracts;

namespace Application.Abstratctions;

public interface ICompanyService
{
    Task<Company> CreateCompany(CompanyRequest companyRequest, User user);
    Task<Company> GetCompany(string userId);
    Task<Company> UpdateCompany(string userId, string name ,string nip);
    Task<Guid> DeleteCompany(Guid companyId);
}