using Application.Abstratctions;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Shared.Response;

namespace Empli.Manager.Controllers
{
    [Authorize]
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
            if (hour == null)
            {
                return BadRequest("Hour is null");
            }

            try
            {
                await _hourService.CreateHour(hour);
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == HourService.DuplicateCalendarDayMessage)
                {
                    return Conflict(ex.Message);
                }

                return BadRequest(ex.Message);
            }

            return Ok(new HourDto{Date = hour.Date, Hours = hour.Hours, Id = hour.WorkerId});
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllHours(Guid workerId)
        {
            var hours = await _hourService.GetAllHours(workerId);
            return Ok(hours);
        }
       
        [HttpGet("period")]
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
            try
            {
                await _hourService.UpdateHour(id, hour, date);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

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