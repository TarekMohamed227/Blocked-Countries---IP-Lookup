using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
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
                // ✅ Construct the correct API URL
                string requestUrl = $"{_baseUrl}?apiKey={_apiKey}&ip={ipAddress}";
                Console.WriteLine($"[DEBUG] Requesting: {requestUrl}");

                // ✅ Make the API request
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

                // ✅ Log HTTP status codes
                Console.WriteLine($"[DEBUG] Response Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] API call failed with status {response.StatusCode}: {errorDetails}");
                    return "API_ERROR";
                }

                // ✅ Read response as JSON object
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] API Response: {responseContent}");

                var jsonResponse = System.Text.Json.JsonDocument.Parse(responseContent).RootElement;

                // ✅ Extract "country_code2" from JSON
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
