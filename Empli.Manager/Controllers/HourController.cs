using Application.Abstratctions;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.Response;

namespace Empli.Manager.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class HourContoller : Controller
    {
        private readonly IHourService _hourService;

        public HourContoller (IHourService hourService)
        {
            _hourService = hourService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHour([FromBody] HourRequest hour)
        {
            
            await _hourService.CreateHour(hour);
            if (hour == null)
            {
                return BadRequest("Hour is null");
            }
            
            return Ok(new HourDto{Date = hour.Date, Hours = hour.Hours, Id = hour.WorkerId});
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHours(Guid workerId)
        {
            var hours = await _hourService.GetAllHours(workerId);
            return Ok(hours);
        }
       
        [HttpGet]
        public async Task<IActionResult> GetHoursPeriod(Guid id, DateTime dateStart, DateTime dateEnd)
        {
            if (dateEnd < dateStart)
            {
                return BadRequest("End date cannot be less than start date");
            }
            var hours = await _hourService.GetHours(id, dateStart, dateEnd);
            return Ok(hours);
            
        }
        
        
        [HttpPatch]
        public async Task<ActionResult<HourDto>> UpdateHour(Guid id , float hour , DateTime date)
        {
   
            if (hour == null)
            {
                return BadRequest("Not found");
            }
            await _hourService.UpdateHour(id, hour, date);

            return Ok();
            

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteHour(Guid id , DateTime date)
        {
            if (id == null)
            {
                BadRequest("Not Found");
            }

            await _hourService.DeleteHour(id, date);
            return Ok();
        }
        
        


    }
    
}