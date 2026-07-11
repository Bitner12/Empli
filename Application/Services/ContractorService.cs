using Application.Abstratctions;
using Domain.Abstractions.Interfaces.Repositories;
using Domain.Entities;
using Shared.Contracts;

namespace Application.Services
{
    public class ContractorService : IContractorService
    {
        private readonly IContractorRepository _repository;

        public ContractorService(IContractorRepository repository)
        {
            _repository = repository;
        }

        public Task<Contractor> Create(Guid companyId, ContractorRequest request)
            => _repository.Create(companyId, request);

        public Task<Contractor?> Update(Guid id, Guid companyId, ContractorRequest request)
            => _repository.Update(id, companyId, request);

        public Task<List<Contractor>> GetByCompany(Guid companyId)
            => _repository.GetByCompany(companyId);

        public Task<List<Contractor>> GetByWorker(Guid workerId)
            => _repository.GetByWorker(workerId);

        public Task Delete(Guid id, Guid companyId)
            => _repository.Delete(id, companyId);
    }
}