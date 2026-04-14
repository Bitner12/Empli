using Application.Abstratctions;
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
    public class ManagerController : Controller
    {


        private readonly IMangerService _mangerService;

        public ManagerController(IMangerService mangerService)
        {

            _mangerService = mangerService;

        }

        [HttpPost("addWorker")]
        public async Task<IActionResult> AddWorker([FromBody] WorkerRequest workerRequest)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var newWorker = await _mangerService.AddWorker(workerRequest, userId);
            if (newWorker == null)
            {
                return BadRequest("Failed to add worker");
            }

            return Ok("Worker added to company");
        }


        [HttpGet("getWorkers")]
        public async Task<IActionResult> GetWorkers()
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var workers = await _mangerService.GetWorkers(userId);
            if (workers == null)
            {
                return BadRequest("Failed to get workers");
            }
            return Ok(workers);
        }

        [HttpGet("getWorkerByPesel")]

        public async Task<IActionResult> GetWorkerByPesel([FromQuery] string pesel)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var worker = await _mangerService.GetWorkerByPesel(pesel, userId);
            if (worker == null)
            {
                return BadRequest("Failed to get worker");
            }
            return Ok(worker);

        }

        [HttpPut("updateWorker")]

        public async Task<IActionResult> UpdateWorker([FromQuery] Guid id, [FromQuery] string firstName, [FromQuery] string lastName, [FromQuery] decimal costPerHour)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var updatedWorker = await _mangerService.UpdateWorker(id, firstName, lastName, costPerHour, userId);
            if (updatedWorker == null)
            {
                return BadRequest("Failed to update worker");
            }
            return Ok(updatedWorker);
        }


        [HttpDelete("deleteWorker")]
        public async Task<IActionResult> DeleteWorker([FromQuery] Guid id)
        {
            var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
            var result = await _mangerService.DeleteWorker(id, userId);
            if (!result)
            {
                return BadRequest("Failed to delete worker");
            }
            return Ok("Worker deleted successfully");
        }

    }
}

