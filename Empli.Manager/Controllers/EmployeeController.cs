using Application.Services;
using Domain.Entities;
using Empli.Manager.Utiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;

namespace Empli.Manager.Controllers

{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController  : Controller
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeRequest request, int id)
        {
            await _employeeService.UpdateEmployee(id,request);
            return Ok();
        }
    
    }  
}