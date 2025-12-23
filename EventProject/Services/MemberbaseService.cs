using System.Net.Http.Headers;

namespace EventProject.Services
{
    public class MemberbaseService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MemberbaseService(IConfiguration configuration,HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress =
                new Uri("https://demo-log.memberbase-sandbox.com");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _configuration["Membership:Key"]);
        }

        public async Task<bool> CreateOrGetContact(string name, string email)
        {
            var payload = new
            {
                firstName = name,
                emailAddress = email
            };

            var response = await _httpClient
                .PostAsJsonAsync("/api/contacts", payload);

            return response.IsSuccessStatusCode;
        }
    }
}
