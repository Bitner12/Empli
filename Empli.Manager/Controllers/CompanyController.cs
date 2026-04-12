
using Application.Abstratctions;
using Application.Services;
using Domain.Enums;
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
    public class CompanyController : Controller

    {
    private readonly ICompanyService _companyService;
    private readonly IUserService _userService;

    public CompanyController(ICompanyService companyService, IUserService userService)
    {
        _companyService = companyService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyRequest companyRequest)
    {
        var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
        
        var user = await _userService.GetUser(userId);
        
        var company = await _companyService.CreateCompany(companyRequest, user);
        if (company == null)
        {
            return BadRequest();
        }
        var updateUser = await _userService.UpdateUser( user , UserType.Company);
        if (updateUser == null)
        {
            return BadRequest("Profile has already been created.");
        }

        return Ok(new CompanyResponse{Id = company.Id , Name = company.Name , Nip = company.Nip});
    }

    [HttpGet]
    public async Task<IActionResult> GetCompany()
    {
        var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();
    
        var company = await _companyService.GetCompany(userId);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }


    [HttpPut]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyRequest companyRequest)
    {
        await _companyService.UpdateCompany(companyRequest.UserId,companyRequest.Name,companyRequest.Nip);
        return Ok();
    }
    
    }
}