
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

    [HttpPost("create")]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyRequest companyRequest)
    { 
        var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
        
        var user = await _userService.GetUser(userId);
            if (user.UserType != 0) 
            {
                return BadRequest("Profile has already been created.");
            }


            var company = await _companyService.CreateCompany(companyRequest.Name,companyRequest.Nip,userId,user);
        if (company == null)
        {
            return BadRequest();
        }
            var updateUser = await _userService.UpdateUser(user, UserType.Company, company.Id, company, null);
            if (updateUser == null)
            {
                return BadRequest("Profile has already been created.");
            }

            return Ok(new CompanyResponse{Id = company.Id , Name = company.Name , Nip = company.Nip});
    }

    [HttpGet("get")]
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


    [HttpPut("update")]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyRequest companyRequest)
    {
        var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
        await _companyService.UpdateCompany(userId,companyRequest.Name,companyRequest.Nip);
        return Ok();
    }
    
    }
}