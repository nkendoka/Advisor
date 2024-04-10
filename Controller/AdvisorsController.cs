
using AdvisorManagement.Infrastructure.Persistence;
using AdvisorManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvisorManagement.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvisorsController : ControllerBase
    {
        private readonly IAdvisorRepository _advisorRepository;

        public AdvisorsController(IAdvisorRepository advisorRepository)
        {
            _advisorRepository = advisorRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAdvisor(Advisor advisor)
        {
            await _advisorRepository.AddAsync(advisor);
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

                var advisorToUpdate = await _advisorRepository.GetAsync(advisorId);

                if (advisorToUpdate == null)
                {
                    return NotFound($"Advisor with Id = {advisorId} not found");
                }

                return Ok(_advisorRepository.UpdateAsync(advisor));
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
            return Ok(_advisorRepository.GetAsync(advisorId));
        }               

        [HttpDelete("{advisorId:int}")]
        public async Task<ActionResult> DeleteAdvisor(int advisorId)
        {
            try
            {
                var advisorToDelete = await _advisorRepository.GetAsync(advisorId);

                if (advisorToDelete == null)
                {
                    return NotFound($"Advisor with Id = {advisorId} not found");
                }

                await _advisorRepository.DeleteAsync(advisorId);
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
