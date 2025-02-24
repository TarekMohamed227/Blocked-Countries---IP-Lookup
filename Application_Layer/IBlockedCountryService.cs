using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain_Layer;

namespace Application_Layer
{
    public interface IBlockedCountryService
    {
        Task<bool> BlockCountryAsync(string countryCode, int? durationMinutes = null);
        Task<bool> UnblockCountryAsync(string countryCode);
        List<BlockedCountry> GetBlockedCountries();
        Task<bool> IsIPBlockedAsync(string ipAddress);

        // ✅ Add these missing methods
        bool IsCountryBlocked(string countryCode);
        void LogBlockedAttempt(string ip, string countryCode, bool blocked, string userAgent);
        List<BlockedAttemptLog> GetBlockedAttempts();
    }
}
