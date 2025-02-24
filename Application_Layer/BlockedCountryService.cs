using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain_Layer;
using Infrastructure_Layer;

namespace Application_Layer
{
    public class BlockedCountryService : IBlockedCountryService
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries;
        private readonly IGeolocationService _geolocationService;

        private static readonly List<BlockedAttemptLog> _blockedAttempts = new();

        public BlockedCountryService(IGeolocationService geolocationService)
        {
            _blockedCountries = new ConcurrentDictionary<string, BlockedCountry>();
            _geolocationService = geolocationService;
        }

        public async Task<bool> BlockCountryAsync(string countryCode, int? durationMinutes = null)
        {
            if (_blockedCountries.ContainsKey(countryCode))
            {
                Console.WriteLine($"[DEBUG] Country {countryCode} is already blocked.");
                return false;
            }

            var blockedCountry = new BlockedCountry
            {
                CountryCode = countryCode,
                ExpirationTime = durationMinutes.HasValue ? DateTime.UtcNow.AddMinutes(durationMinutes.Value) : null
            };

            bool added = _blockedCountries.TryAdd(countryCode, blockedCountry);

            Console.WriteLine($"[DEBUG] Blocking Country: {countryCode}, Added: {added}, Total Blocked Countries: {_blockedCountries.Count}");

            return added;
        }

        public Task<bool> UnblockCountryAsync(string countryCode)
        {
            return Task.FromResult(_blockedCountries.TryRemove(countryCode, out _));
        }

        public List<BlockedCountry> GetBlockedCountries()
        {
            Console.WriteLine($"Blocked Countries Count: {_blockedCountries.Count}");

            foreach (var country in _blockedCountries.Values)
            {
                Console.WriteLine($"Blocked Country: {country.CountryCode}, Expiration: {country.ExpirationTime}");
            }

            return _blockedCountries.Values.ToList();
        }

        public async Task<bool> IsIPBlockedAsync(string ipAddress)
        {
            var countryCode = await _geolocationService.GetCountryCodeByIPAsync(ipAddress);
            return _blockedCountries.TryGetValue(countryCode, out var country) && country.IsTemporarilyBlocked;
        }

        public bool IsCountryBlocked(string countryCode)
        {
            if (_blockedCountries.TryGetValue(countryCode, out var country))
            {
                return country.ExpirationTime == null || country.ExpirationTime > DateTime.UtcNow;
            }
            return false;
        }

        public void LogBlockedAttempt(string ip, string countryCode, bool blocked, string userAgent)
        {
            _blockedAttempts.Add(new BlockedAttemptLog
            {
                IpAddress = ip,
                CountryCode = countryCode,
                BlockedStatus = blocked,
                Timestamp = DateTime.UtcNow,
                UserAgent = userAgent
            });
        }

       
        public List<BlockedAttemptLog> GetBlockedAttempts()
        {
            return _blockedAttempts.OrderByDescending(log => log.Timestamp).ToList();
        }
    }
}
