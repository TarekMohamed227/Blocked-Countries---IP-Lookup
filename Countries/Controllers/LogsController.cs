using Application_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocked_Countries.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly IBlockedCountryService _blockedCountryService; //  Service for retrieving blocked attempt logs

        public LogsController(IBlockedCountryService blockedCountryService)
        {
            _blockedCountryService = blockedCountryService;
        }

       
        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = _blockedCountryService.GetBlockedAttempts()
                        .Skip((page - 1) * pageSize) //  Skips records for pagination
                        .Take(pageSize) //  Takes the required number of records
                        .ToList();

            return Ok(new { page, pageSize, logs }); //  Returns paginated list of logs
        }
    }
}
