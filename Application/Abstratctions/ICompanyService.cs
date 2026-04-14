using Domain.Entities;
using Shared.Contracts;

namespace Application.Abstratctions;

public interface ICompanyService
{
    Task<Company> CreateCompany(string name,string nip, string userId, User user);
    Task<Company> GetCompany(string userId);
    Task<Company> UpdateCompany(string userId, string name ,string nip);
    Task<Guid> DeleteCompany(Guid companyId);
    Task<Company> GetByNip(string nip);
}