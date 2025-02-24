using Application_Layer;
using Infrastructure_Layer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocked_Countries.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IGeolocationService _geoService;
        private readonly IBlockedCountryService _blockedCountryService;

        public IpController(IGeolocationService geoService, IBlockedCountryService blockedCountryService)
        {
            _geoService = geoService;
            _blockedCountryService = blockedCountryService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> LookupIP([FromQuery] string? ipAddress = null)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
                {
                    ipAddress = "8.8.8.8"; // Default Google public IP
                }
            }

            var countryCode = await _geoService.GetCountryCodeByIPAsync(ipAddress);
            return Ok(new { ipAddress, countryCode });
        }

       
        [HttpGet("check-block")]
        public async Task<IActionResult> IsBlocked()
        {
            string userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(userIp) || userIp == "::1")
            {
                userIp = "8.8.8.8"; // Default for local testing
            }

            var countryCode = await _geoService.GetCountryCodeByIPAsync(userIp);
            bool isBlocked = _blockedCountryService.IsCountryBlocked(countryCode);

            _blockedCountryService.LogBlockedAttempt(userIp, countryCode, isBlocked, Request.Headers["User-Agent"]);

            if (isBlocked)
            {
                return Unauthorized($"Access denied from {countryCode}");
            }

            return Ok(new { message = "Access granted", ip = userIp, country = countryCode });
        }
    }
}
