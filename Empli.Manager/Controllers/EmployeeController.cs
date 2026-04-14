using Application.Abstratctions;
using Application.Services;
using Domain.Entities;
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
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUserService _userService;
        public EmployeeController(IEmployeeService employeeService, IUserService userService)
        {
            _employeeService = employeeService;
            _userService = userService;
        }

        [HttpPost("create")]

        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRequest request)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var user = await _userService.GetUser(userId);
            if (user.UserType != 0)
            {
                return BadRequest("Profile has already been created.");
            }
            var employee = await _employeeService.EmployeeCreate(request.FirstName,request.LastName, request.CostPerHour ,user,request.Pesel);
            if (employee == null)
            {
                return BadRequest ();
            }
            var updateUser =  await _userService.UpdateUser(user, UserType.Employee,employee.Id,null, employee);

            if (updateUser == null)
            {
                return BadRequest("Profile has already been created.");
            }


            return Ok(new EmployeeResponse {Id = employee.Id, FirstName = employee.FirstName, LastName = employee.LastName, UserId = employee.UserId } );
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetEmployee()
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var employee = await _employeeService.EmployeeGet(userId);
            if (employee == null)
            {
                return NotFound();
            }

            var linked = employee.Worker?.CompanyId != null;
            return Ok(new EmployeeProfileResponse
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Pesel = employee.Pesel,
                CostPerHour = employee.Worker?.CostPerHour,
                WorkerId = employee.WorkerId,
                IsLinkedToCompany = linked
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeRequest request)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var updated = await _employeeService.EmployeeUpdateProfile(userId, request.FirstName, request.LastName, request.CostPerHour);
            if (updated == null)
            {
                return BadRequest("Нельзя изменить профиль: сотрудник привязан к компании, либо профиль не найден.");
            }

            return Ok(new EmployeeProfileResponse
            {
                Id = updated.Id,
                FirstName = updated.FirstName,
                LastName = updated.LastName,
                Pesel = updated.Pesel,
                CostPerHour = updated.Worker?.CostPerHour,
                WorkerId = updated.WorkerId,
                IsLinkedToCompany = updated.Worker?.CompanyId != null
            });
        }

    }
}
