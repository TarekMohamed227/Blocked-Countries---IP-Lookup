using Application_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocked_Countries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockedCountriesController : ControllerBase
    {
        private readonly IBlockedCountryService _blockedCountryService; //  Service for managing blocked countries

        public BlockedCountriesController(IBlockedCountryService blockedCountryService)
        {
            _blockedCountryService = blockedCountryService;
        }

       
        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] string countryCode, [FromQuery] int? durationMinutes = null)
        {
            if (string.IsNullOrEmpty(countryCode))
                return BadRequest("Country code is required."); //  Validates input

            bool success = await _blockedCountryService.BlockCountryAsync(countryCode, durationMinutes);
            if (!success) return Conflict("Country is already blocked."); //  Prevents duplicate blocks

            return Ok($"Blocked {countryCode}" + (durationMinutes.HasValue ? $" for {durationMinutes} minutes" : ""));
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> UnblockCountry(string countryCode)
        {
            bool success = await _blockedCountryService.UnblockCountryAsync(countryCode);
            if (!success) return NotFound("Country is not blocked."); //  Handles case where country is not blocked

            return Ok($"Unblocked {countryCode}");
        }

        
        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries()
        {
            return Ok(_blockedCountryService.GetBlockedCountries()); //  Returns a list of blocked countries
        }
    }
}
