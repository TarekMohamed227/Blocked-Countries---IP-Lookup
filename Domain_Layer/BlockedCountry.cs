using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer
{
    public class BlockedCountry
    {
<<<<<<< HEAD
        public string CountryCode { get; set; }
        public DateTime? ExpirationTime { get; set; }
=======
        public string CountryCode { get; set; } // Stores country code (e.g.,"US","GB","EG")
        public DateTime? ExpirationTime { get; set; } // If set, it expires after a duration
>>>>>>> f935c37 (Updated API functionality and fixed bugs)

        public bool IsTemporarilyBlocked => ExpirationTime.HasValue && ExpirationTime > DateTime.UtcNow;
    }
}
