using Domain.Entities;
using Shared.Contracts;
using Shared.Response;

namespace Domain.Abstractions.Interfaces.Repositories
{
    public interface IContractorRepository
    {
        Task<Contractor> Create(Guid companyId, ContractorRequest request);
        Task<Contractor?> Update(Guid id, Guid companyId, ContractorRequest request);
        Task<List<Contractor>> GetByCompany(Guid companyId);
        Task<List<Contractor>> GetByWorker(Guid workerId);
        Task Delete(Guid id, Guid companyId);
    }
}