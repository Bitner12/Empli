using Application.Abstratctions;
using Application.Services;
using Domain.Enums;
using Empli.Manager.Utiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.Response;


//Винести логику по созданию токенов в отдельный сервис.
//Винести логику по работе с пользователями в отдельный сервис.
//В ЮзерСервисе сделать метод гет юзер.
//В клайми добавить Ид Юзера + емейл Юзера

namespace Empli.Manager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        private readonly IRegistrationService _registrationService;
        private readonly ILoginService _loginService;
        private readonly IRefreshService _refreshService;
        private readonly IUserService _userService;
        private readonly IEmployeeService _employeeService;

        public UserController(IRegistrationService registrationService, ILoginService loginService, IRefreshService refreshService, IUserService userService, IEmployeeService employeeService)
        {
            _registrationService = registrationService;
            _loginService = loginService;
            _refreshService = refreshService;
            _userService = userService;
            _employeeService = employeeService;

        }




        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUser(userId);
            if (user == null)
            {
                return Unauthorized();
            }

            Guid? workerId = null;
            if (user.UserType == UserType.Employee)
            {
                var employee = await _employeeService.EmployeeGet(userId);
                workerId = employee?.WorkerId;
            }

            return Ok(new UserProfileDto
            {
                UserType = (int)user.UserType,
                Email = user.Email,
                CompanyId = user.CompanyId,
                EmployeeId = user.EmployeeId,
                WorkerId = workerId
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var user = await _registrationService.Registration(registerRequest.Email, registerRequest.Password);
            var login = await _loginService.Login(user.Email, registerRequest.Password);

            return Ok(login);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var login = await _loginService.Login(loginRequest.Email, loginRequest.Password);
            if (login == null)
            {
                return BadRequest();
            }
            return Ok(login);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest();
            }
            await _loginService.Logout(userId);

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (string.IsNullOrWhiteSpace(refreshRequest.RefreshToken))
            {
                return BadRequest();
            }

            var loginResponse = await _refreshService.Refresh(refreshRequest.RefreshToken);
            if (loginResponse == null)
            {
                return BadRequest();
            }

            return Ok(loginResponse);
        }

        [HttpDelete("delete")]
      
        public async Task<IActionResult> Delete(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest("Id is required");

            var result = await _userService.DeleteUser(userId);
            if (result == null)
            {
                return BadRequest();
            }  
            return Ok(result);

        }
    }

}