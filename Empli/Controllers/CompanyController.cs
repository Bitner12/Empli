using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Empli.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompanyController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetResult()
        {
            return Ok();
        }
    }
}
