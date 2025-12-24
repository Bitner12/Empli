
using Empli.Aplication;
using Empli.Aplication.Interfaces;
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
            var user = await _userService.GetUser(loginRequest.Email , loginRequest.Password);
            var token = _tokenService.GenerateToken(user);

            return Ok(new LoginResponse(user.Id, user.Email, user.RefreshToken, token));
        }

        //[HttpPost("refresh")]
        //public async Task<IActionResult> RefreshToken(RefreshRequest refreshRequest)
        //{
        //    var user = await _userManager.FindByIdAsync(refreshRequest.UserId);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    if (user.Expires < DateTime.UtcNow || user.RefreshToken != refreshRequest.RefreshToken)
        //    {
        //        return Unauthorized();
        //    }
        //    var newAccessToken = _jwtService.GenerateToken(user);
        //    user = GenerateRefreshToken(user);
        //    await _userManager.UpdateAsync(user);

        //    return Ok(new LoginResponse(user.Id, user.Email, user.RefreshToken, newAccessToken));

        //}


    }

}
