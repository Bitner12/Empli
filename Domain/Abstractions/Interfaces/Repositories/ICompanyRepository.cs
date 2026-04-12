using Domain.Entities;

namespace Domain.Abstractions.Interfaces.Repositories;

public interface ICompanyRepository
{
    Task<Company> Create(Company company);
    Task<Company> Get(string userId);
    Task<Company> Update(Company company);
    Task<Guid> Delete(Guid companyId);
}