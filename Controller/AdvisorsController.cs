
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

        [HttpPost]
        public async Task<IActionResult> CreateAdvisor(Advisor advisor)
        {
            await _advisorService.AddAdvisorAsync(advisor);
            return Ok(advisor);
        }
    }
}
