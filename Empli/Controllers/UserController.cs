using Empli.Aplication.Interfaces;
using Empli.Aplication.Models;
using Empli.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
//Винести логику по созданию токенов в отдельный сервис.
//Винести логику по работе с пользователями в отдельный сервис.
//В ЮзерСервисе сделать метод гет юзер.
//В клайми добавить Ид Юзера + емейл Юзера

namespace Empli.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        
        private readonly IUserService _userService ;
        private readonly ITokenService _tokenService ;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var (user,result) = await _userService.CreateUser(registerRequest.Email, registerRequest.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description));
            }
            var acsessToken = _tokenService.GenerateToken(user);

            return Ok(new LoginResponse(user.Id, user.Email, user.RefreshToken, acsessToken));
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _userService.Login(loginRequest.Email, loginRequest.Password);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            var token = _tokenService.GenerateToken(result.Value);

            return Ok(new LoginResponse(result.Value.Id, result.Value.Email, result.Value.RefreshToken, token));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshRequest refreshRequest)
        {

            var refreshUser = await _userService.RefreshUser(refreshRequest.UserId, refreshRequest.RefreshToken);
            if (!refreshUser.IsSuccess)
            {
                return Unauthorized(refreshUser.Error);
            }
            var newAccessToken = _tokenService.GenerateToken(refreshUser.Value);

            return Ok(new LoginResponse(refreshUser.Value.Id, refreshUser.Value.Email, refreshUser.Value.RefreshToken, 
                newAccessToken));

        }


    }

}
