
using Empli.Aplication;
using Empli.Domian;
using Empli.Infrastructure;
using Empli.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
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
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _jwtService;
        
        public UserController(UserManager<User> userManager,
            ITokenService jwtService)

        {
            _jwtService = jwtService;
            _userManager = userManager;
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var user = new User { UserName = registerRequest.Email, Email = registerRequest.Email };
            var userWithToken = GenerateRefreshToken(user);
            var acsessToken = _jwtService.GenerateToken(userWithToken);

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            var errors = result.Errors?.FirstOrDefault()?.Description;

            if (!string.IsNullOrEmpty(errors)) 
            {
                return BadRequest(errors);
            }

            return Ok(new LoginResponse(user.Id, user.Email, user.RefreshToken, acsessToken));
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email.ToLowerInvariant());
            if (user == null)
            {
                return Unauthorized();
            }
            var checkPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!checkPassword)
            {
                return Unauthorized();
            }
            var token = _jwtService.GenerateToken(user);
            user = GenerateRefreshToken(user);
            await _userManager.UpdateAsync(user);


            return Ok(new LoginResponse(user.Id, user.Email, user.RefreshToken, token));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshRequest refreshRequest)
        {
            var user = await _userManager.FindByIdAsync(refreshRequest.UserId);
            if (user == null)
            {
                return Unauthorized();
            }
            if (user.Expires < DateTime.UtcNow || user.RefreshToken != refreshRequest.RefreshToken)
            {
                return Unauthorized();
            }
            var newAccessToken = _jwtService.GenerateToken(user);
            user = GenerateRefreshToken(user);
            await _userManager.UpdateAsync(user);

            return Ok(new LoginResponse(user.Id, user.Email, user.RefreshToken, newAccessToken));

        }

        private User GenerateRefreshToken(User user)
        {
            var newRefreshToken = _jwtService.GenerateRefreshToken(user);
            user.RefreshToken = newRefreshToken.Token;
            user.Expires = newRefreshToken.Expires;
            return user;
        }
    }

}
