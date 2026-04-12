using Application.Abstratctions;
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
        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpPost]
        public async Task<IActionResult> Creat([FromBody] WorkerRequest worker, Guid companyId)
        {
            
            await _workerService.CreatWorker(worker, companyId);
            if (string.IsNullOrWhiteSpace(worker.FirstName))
            {
                return BadRequest("Имя не может быть пустым");
            }

            if (string.IsNullOrWhiteSpace(worker.LastName))
            {
                return BadRequest("Фамилия не может быть пустая");
            }

            if (worker.CostPerHour <= 0)
            {
                return BadRequest("Недопустимая стоимость за час.");
            }
            if (worker == null)
            {
                return BadRequest("Worker is null");
            }

            return Ok();
        }

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
