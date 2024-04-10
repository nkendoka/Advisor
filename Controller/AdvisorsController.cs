
using AdvisorManagement.Model;
using AdvisorManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvisorManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorsController : ControllerBase
    {
        private readonly AdvisorService _advisorService;

        public AdvisorsController(AdvisorService advisorService)
        {
            _advisorService = advisorService;
        }
        public AdvisorService Get_advisorService()
        {
            return _advisorService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvisor(Advisor advisor)
        {
            await _advisorService.AddAdvisorAsync(advisor);
            return Ok(advisor);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Advisor>> UpdateAdvisor(int advisorId, Advisor advisor)
        {
            try
            {
                if (advisorId != advisor.Id)
                {
                    return BadRequest("Advisor ID mismatch");
                }

                var advisorToUpdate = _advisorService.GetAdvisor(advisorId);

                if (advisorToUpdate == null)
                {
                    return NotFound($"Advisor with Id = {advisorId} not found");
                }

                return Ok(_advisorService.UpdateAdvisorAsync(advisor));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

        [HttpGet("{advisorId:int}")]
        public ActionResult<Advisor> GetAdvisor(int advisorId)
        {
            return Ok(_advisorService.GetAdvisor(advisorId));
        }               

        [HttpDelete("{advisorId:int}")]
        public async Task<ActionResult<Advisor>> DeleteAdvisor(int advisorId)
        {
            try
            {
                var advisorToDelete = _advisorService.GetAdvisor(advisorId);

                if (advisorToDelete == null)
                {
                    return NotFound($"Advisor with Id = {advisorId} not found");
                }

                await _advisorService.DeleteAdvisorAsync(advisorToDelete);
                return Ok(advisorToDelete);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
