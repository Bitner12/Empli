using Domain.Entities;
using Shared.Contracts;

namespace Application.Abstratctions
{
    public interface IContractorService
    {
        Task<Contractor> Create(Guid companyId, ContractorRequest request);
        Task<Contractor?> Update(Guid id, Guid companyId, ContractorRequest request);
        Task<List<Contractor>> GetByCompany(Guid companyId);
        Task<List<Contractor>> GetByWorker(Guid workerId);
        Task Delete(Guid id, Guid companyId);
    }
}