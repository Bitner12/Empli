
using Empli.Aplication;
using Empli.Domian;
using Empli.Infrastructure;
using Empli.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


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
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            return Ok("Account created");
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
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
            var refreshToken = _jwtService.GenerateRefreshToken(user);
            user.RefreshToken = refreshToken.Token;
            user.Expires = refreshToken.Expires;
            await _userManager.UpdateAsync(user);

            return Ok(new LoginResponse()
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = refreshToken

            });
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
            var newRefreshToken = _jwtService.GenerateRefreshToken(user);
            user.RefreshToken = newRefreshToken.Token;
            user.Expires = newRefreshToken.Expires;
            await _userManager.UpdateAsync(user);
  
            return Ok(new LoginResponse()
            {
                UserId = user.Id,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
