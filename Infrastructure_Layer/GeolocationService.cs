using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
using System.Net;
using System.Net.Http.Json;
=======
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Infrastructure_Layer
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public GeolocationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeoAPI:ApiKey"];
            _baseUrl = configuration["GeoAPI:BaseUrl"] ?? "https://api.ipgeolocation.io/ipgeo";

        }

        public async Task<string> GetCountryCodeByIPAsync(string ipAddress)
        {
            try
            {
<<<<<<< HEAD
                // ✅ Construct the correct API URL
                string requestUrl = $"{_baseUrl}?apiKey={_apiKey}&ip={ipAddress}";
                Console.WriteLine($"[DEBUG] Requesting: {requestUrl}");

                // ✅ Make the API request
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                // ✅ Log HTTP status codes
=======
                //  Construct the correct API URL
                string requestUrl = $"{_baseUrl}?apiKey={_apiKey}&ip={ipAddress}";
                Console.WriteLine($"[DEBUG] Requesting: {requestUrl}");

                //  Make the API request
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                //  Log HTTP status codes
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
                Console.WriteLine($"[DEBUG] Response Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] API call failed with status {response.StatusCode}: {errorDetails}");
                    return "API_ERROR";
                }

<<<<<<< HEAD
                // ✅ Read response as JSON object
=======
                //  Read response as JSON object
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] API Response: {responseContent}");

                var jsonResponse = System.Text.Json.JsonDocument.Parse(responseContent).RootElement;

<<<<<<< HEAD
                // ✅ Extract "country_code2" from JSON
=======
                //  Extract "country_code2" from JSON
>>>>>>> f935c37 (Updated API functionality and fixed bugs)
                if (jsonResponse.TryGetProperty("country_code2", out var countryCodeElement))
                {
                    string countryCode = countryCodeElement.GetString();
                    Console.WriteLine($"[DEBUG] Received country code: {countryCode}");
                    return countryCode ?? "Unknown";
                }
                else
                {
                    Console.WriteLine("[ERROR] 'country_code2' field not found in API response.");
                    return "Unknown";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception while fetching geolocation: {ex.Message}");
                return "Unknown";
            }
        }


    }
}
