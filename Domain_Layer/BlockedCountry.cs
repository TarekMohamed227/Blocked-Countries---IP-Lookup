using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer
{
    public class BlockedCountry
    {
        public string CountryCode { get; set; }
        public DateTime? ExpirationTime { get; set; }

        public bool IsTemporarilyBlocked => ExpirationTime.HasValue && ExpirationTime > DateTime.UtcNow;
    }
}
