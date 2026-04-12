using Application.Abstratctions;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;


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

      public UserController(IRegistrationService registrationService, ILoginService loginService)
      {
          _registrationService = registrationService;
          _loginService = loginService;
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
        public async Task<IActionResult> Logout()
        {
            
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest refreshRequest)
        {
            
            
            return Ok();
        }
        
    }

}