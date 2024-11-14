using ST10296167_PROG6212_POE.Models;
using System.Text.Json;
using Newtonsoft.Json;

namespace ST10296167_PROG6212_POE.Services
{
    public class ClaimApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;

        public ClaimApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUri = configuration.GetValue<string>("ApiBaseUri");  // Get the base URI from appsettings
        }

        public async Task<List<Claims>> GetVerifiedClaimsAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUri}/api/ClaimApi/Verify");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Claims>>();
            }

            // Handle the failure (optional)
            return new List<Claims>();
        }

        public async Task<SortedClaims> GetClaimsPCAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUri}/api/ClaimApi/GetClaimsPC");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SortedClaims>(content);
            }
            else
            {
                return new SortedClaims();
            }
        }

        public async Task<SortedClaims> GetClaimsAMAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUri}/api/ClaimApi/GetClaimsAM");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SortedClaims>(content);
            }
            else
            {
                return new SortedClaims();
            }
        }

    }
}
