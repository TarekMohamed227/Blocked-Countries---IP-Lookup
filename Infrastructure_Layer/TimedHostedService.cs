using System;
using System.Threading;
using System.Threading.Tasks;
using Application_Layer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure_Layer
{
    public class TimedHostedService : BackgroundService
    {
        private readonly IBlockedCountryService _blockedCountryService;
        private readonly ILogger<TimedHostedService> _logger;

        public TimedHostedService(IBlockedCountryService blockedCountryService, ILogger<TimedHostedService> logger)
        {
            _blockedCountryService = blockedCountryService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for expired blocked countries...");
                _blockedCountryService.RemoveExpiredBlocks(); // Implement this method in service
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Runs every 5 minutes
            }
        }
    }
}
