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
<<<<<<< HEAD
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries;
        private readonly IGeolocationService _geolocationService;

        private static readonly List<BlockedAttemptLog> _blockedAttempts = new();
=======
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries; //  Stores blocked countries in-memory
        private readonly IGeolocationService _geolocationService; //  External service to fetch country code from IP

        private static readonly List<BlockedAttemptLog> _blockedAttempts = new(); //  Stores logs of blocked attempts
>>>>>>> f935c37 (Updated API functionality and fixed bugs)

        public BlockedCountryService(IGeolocationService geolocationService)
        {
            _blockedCountries = new ConcurrentDictionary<string, BlockedCountry>();
            _geolocationService = geolocationService;
        }

        public async Task<bool> BlockCountryAsync(string countryCode, int? durationMinutes = null)
        {
<<<<<<< HEAD
            if (_blockedCountries.ContainsKey(countryCode))
            {
                Console.WriteLine($"[DEBUG] Country {countryCode} is already blocked.");
                return false;
            }

=======
            //  Validate durationMinutes (Must be between 1 and 1440)
            if (durationMinutes.HasValue && (durationMinutes < 1 || durationMinutes > 1440))
            {
                throw new ArgumentException("Duration must be between 1 and 1440 minutes (24 hours).");
            }

            //  2️⃣ Validate country code 
            if (!IsValidCountryCode(countryCode))
            {
                throw new ArgumentException("Invalid country code.");
            }

            // = Prevent duplicate temporary blocks
            if (_blockedCountries.TryGetValue(countryCode, out var existingBlock))
            {
                if (existingBlock.ExpirationTime.HasValue)
                {
                    throw new InvalidOperationException($"Country {countryCode} is already temporarily blocked.");
                }
            }

            // = Create and store blocked country entry
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
            var blockedCountry = new BlockedCountry
            {
                CountryCode = countryCode,
                ExpirationTime = durationMinutes.HasValue ? DateTime.UtcNow.AddMinutes(durationMinutes.Value) : null
            };

<<<<<<< HEAD
            bool added = _blockedCountries.TryAdd(countryCode, blockedCountry);

            Console.WriteLine($"[DEBUG] Blocking Country: {countryCode}, Added: {added}, Total Blocked Countries: {_blockedCountries.Count}");

            return added;
=======
            return _blockedCountries.TryAdd(countryCode, blockedCountry); //  Adds to the dictionary if not already blocked
        }

        //  check if a country code is valid
        private bool IsValidCountryCode(string countryCode)
        {
            HashSet<string> validCountryCodes = new HashSet<string>
            {
                "US", "GB", "EG", "CA", "FR", "DE", "IN", "AU", "BR", "CN", "JP", "ZA", "IT", "RU", "MX"
            };

            return validCountryCodes.Contains(countryCode.ToUpper());
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
        }

        public Task<bool> UnblockCountryAsync(string countryCode)
        {
<<<<<<< HEAD
            return Task.FromResult(_blockedCountries.TryRemove(countryCode, out _));
=======
            return Task.FromResult(_blockedCountries.TryRemove(countryCode, out _)); //  Removes country from blocked list
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
        }

        public List<BlockedCountry> GetBlockedCountries()
        {
<<<<<<< HEAD
            Console.WriteLine($"Blocked Countries Count: {_blockedCountries.Count}");

            foreach (var country in _blockedCountries.Values)
            {
                Console.WriteLine($"Blocked Country: {country.CountryCode}, Expiration: {country.ExpirationTime}");
            }

            return _blockedCountries.Values.ToList();
=======
            Console.WriteLine($"Blocked Countries Count: {_blockedCountries.Count}"); //  Logs the number of blocked countries

            foreach (var country in _blockedCountries.Values)
            {
                Console.WriteLine($"Blocked Country: {country.CountryCode}, Expiration: {country.ExpirationTime}"); //  Logs details
            }

            return _blockedCountries.Values.ToList(); //  Returns a list of blocked countries
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
        }

        public async Task<bool> IsIPBlockedAsync(string ipAddress)
        {
            var countryCode = await _geolocationService.GetCountryCodeByIPAsync(ipAddress);
<<<<<<< HEAD
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
=======
            return _blockedCountries.ContainsKey(countryCode); //  Checks if blocked 
        }

>>>>>>> f935c37 (Updated API functionality and fixed bugs)

        public void LogBlockedAttempt(string ip, string countryCode, bool blocked, string userAgent)
        {
            _blockedAttempts.Add(new BlockedAttemptLog
            {
<<<<<<< HEAD
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
=======
                IpAddress = ip, 
                CountryCode = countryCode, 
                BlockedStatus = blocked, 
                Timestamp = DateTime.UtcNow, 
                UserAgent = userAgent 
            });
        }

        public List<BlockedAttemptLog> GetBlockedAttempts()
        {
            return _blockedAttempts.OrderByDescending(log => log.Timestamp).ToList(); //  Returns logs sorted by latest attempts
        }

        public void RemoveExpiredBlocks()
        {
            var expiredCountries = _blockedCountries
                .Where(x => x.Value.ExpirationTime.HasValue && x.Value.ExpirationTime <= DateTime.UtcNow)
                .Select(x => x.Key)
                .ToList(); //  Finds all expired blocked countries

            foreach (var country in expiredCountries)
            {
                _blockedCountries.TryRemove(country, out _); //  Removes expired blocked countries
            }
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
        }
    }
}
