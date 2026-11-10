using System.Net.Http.Headers;

namespace Packet_Sniffers.Services
{
    public class NpsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public NpsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = Environment.GetEnvironmentVariable("NPS_API_KEY")
                ?? throw new InvalidOperationException(
                    "NPS_API_KEY was not found. Check your .env file.");

            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
        }
    }
}