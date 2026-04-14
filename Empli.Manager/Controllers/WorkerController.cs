using Application.Abstratctions;
using Empli.Manager.Utiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;

namespace Empli.Manager.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : Controller
    {

        private readonly IWorkerService _workerService;
        private readonly IUserService _userService;
        
        public WorkerController(IWorkerService workerService, IUserService userService  )
        {
            _userService = userService;
            _workerService = workerService;
           
        }

        

        //[HttpPost("create")]
        //public async Task<IActionResult> Create([FromBody] WorkerRequest worker)
        //{

        //    var userId = JwtParser.GetUserIdFromHttpContext(HttpContext);
        //    var user = await _userService.GetUser(userId);

        //    await _workerService.CreatWorker(worker, user.CompanyId);
        //    if (string.IsNullOrWhiteSpace(worker.FirstName))
        //    {
        //        return BadRequest("Имя не может быть пустым");
        //    }

        //    if (string.IsNullOrWhiteSpace(worker.LastName))
        //    {
        //        return BadRequest("Фамилия не может быть пустая");
        //    }

        //    if (worker.CostPerHour <= 0)
        //    {
        //        return BadRequest("Недопустимая стоимость за час.");
        //    }
        //    if (worker == null)
        //    {
        //        return BadRequest("Worker is null");
        //    }

        //    return Ok();
        //}

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName(string searchName , DateTime? startDate, DateTime? endDate)
        {

            var response = await _workerService.GetWorkerStatistics(searchName, startDate, endDate);
                if (response == null)
            {
                return BadRequest("Not found");
            }
            return Ok(response);
             
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(Guid id,[FromBody] WorkerRequest worker)
        {
            await _workerService.UpdateWorker(id, worker.FirstName,worker.LastName, worker.CostPerHour);
            if (worker == null)
            {
                return BadRequest("Employee not found");
            }

            return Ok(worker);
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _workerService.DeleteWorker(id);
           
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(Guid companyId)
        {

            var response = await _workerService.GetAllAsync(companyId);
            if (response == null)
            {
                return BadRequest("Not found");
            }
            return Ok(response);

        }
        [HttpGet("by-Id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _workerService.GetById(id);
            if (response == null)
            {
                return BadRequest("Not found");
            }
            return Ok(response);

        }
    }
}
