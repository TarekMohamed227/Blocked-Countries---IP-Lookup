using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer
{
    public interface IGeolocationService
    {
        Task<string> GetCountryCodeByIPAsync(string ipAddress);
    }
}
