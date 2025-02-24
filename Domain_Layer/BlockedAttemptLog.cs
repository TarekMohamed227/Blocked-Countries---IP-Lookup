using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer
{
    public class BlockedAttemptLog
    {
        public string IpAddress { get; set; }
        public string CountryCode { get; set; }
        public bool BlockedStatus { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserAgent { get; set; }
    }
}
