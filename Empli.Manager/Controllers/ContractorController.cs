using Application.Abstratctions;
using Empli.Manager.Utiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.Response;

namespace Empli.Manager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContractorController : Controller
    {
        private readonly IContractorService _contractorService;
        private readonly ICompanyService _companyService;

        public ContractorController(IContractorService contractorService, ICompanyService companyService)
        {
            _contractorService = contractorService;
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContractorRequest request)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var company = await _companyService.GetCompany(userId);
            if (company == null) return Forbid();

            var contractor = await _contractorService.Create(company.Id, request);
            return Ok(new ContractorDto { Id = contractor.Id, Name = contractor.Name, CompanyId = contractor.CompanyId, RatePerHour = contractor.RatePerHour });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetByCompany()
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var company = await _companyService.GetCompany(userId);
            if (company == null) return Forbid();

            var list = await _contractorService.GetByCompany(company.Id);
            return Ok(list.Select(c => new ContractorDto { Id = c.Id, Name = c.Name, CompanyId = c.CompanyId, RatePerHour = c.RatePerHour }));
        }

        [HttpGet("byWorker")]
        public async Task<IActionResult> GetByWorker(Guid workerId)
        {
            var list = await _contractorService.GetByWorker(workerId);
            return Ok(list.Select(c => new ContractorDto { Id = c.Id, Name = c.Name, CompanyId = c.CompanyId, RatePerHour = c.RatePerHour }));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] ContractorRequest request)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var company = await _companyService.GetCompany(userId);
            if (company == null) return Forbid();

            var contractor = await _contractorService.Update(id, company.Id, request);
            if (contractor == null) return NotFound();
            return Ok(new ContractorDto { Id = contractor.Id, Name = contractor.Name, CompanyId = contractor.CompanyId, RatePerHour = contractor.RatePerHour });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid id)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var company = await _companyService.GetCompany(userId);
            if (company == null) return Forbid();

            await _contractorService.Delete(id, company.Id);
            return Ok();
        }
    }
}