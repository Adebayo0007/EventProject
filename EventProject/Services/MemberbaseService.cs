using EventProject.Models;
using System.Net.Http.Headers;

namespace EventProject.Services
{
    public class MemberbaseService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        string key = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIxIiwianRpIjoiNWJmMmExZTRjNDMzNWY5ZGY3ZjE2YmZkY2JlNGE4OWU0ZGUwNzMwYTQ2OWMyYmEzODZmODMyYWE0M2I3MDIzYWNkMzE5NzhiNzNiZjc4MzYiLCJpYXQiOjE3NjEzNzY0NzkuNDI3MjAzLCJuYmYiOjE3NjEzNzY0NzkuNDI3MjA2LCJleHAiOjE5MTkxNDI4NzkuNDEzMTM1LCJzdWIiOiIiLCJzY29wZXMiOlsiKiJdfQ.CrsmfKta_xznCQqizkfckm31Fa_5WNGbz_xr7SMks5MHPrWYSKyIbikvIZcsUZnlMLh9fL3GdDlrstybDJqUrjnVdUS8itjHAph_Zp5ZCrB10vi9NTUfBzb51p5-Qc64ZvQrjmo7agZ2dNRy_sbHqSDckntVHu9NsnFcTjXzB50ad2kPv5WPUdYLITEG0jbst61XOrsS5mOeb5HxAnk4wGMRkV8e8rPTb5If_tvjVu1RdN-O1EBa47UMJ927XXUMrMxnGVEMcD_ITr0HoSKxrDBucphe6tG_uIpyEiHNjZcj-on9Tf91583wHj612qD4mr3c7QAA0KRDbMsHkypttGyDpnhsi4gQfVVACnrlI1mTD67pWAhVcde59i1qJ7XFSPObbXPLf1E1mYO1cY4Rj966h_XcNv1AtGezG_kKbOw9yCluaI5_5Putd402-bl-cHRzFCZo6V46mXVpwQHGj_qxk0zn0uNEX4Zapmf6_DnxqoGtPHfsyfMCci8z0lEJR7v0aB0yOqot3WIwTb9QF3rzsC279DAAgQrxvkmAIDAtFRs0uqiwckPODtI1h-nJt9oIz3v8FDB4EDIc8qJuFuNEPjDpcn9Lvp_IMKA3TtyyBRERmMzm9roeVxGKxh2drMU9yJ8X5w-h8A7lrIlNGxEPMJVwoNMny-HWUNV6HA8";

        public MemberbaseService(IConfiguration configuration,HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress =
                new Uri("https://demo-log.memberbase-sandbox.com");
            /*
                   _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", _configuration["Membership:Key"]);*/


            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", key);
        }

        public async Task<MemberbaseResult> CreateOrGetContact(string name, string email)
        {
            var payload = new
            {
                firstName = name,
                emailAddress = email
            };

            try
            {
                var response = await _httpClient
                    .PostAsJsonAsync("/api/contacts", payload);

                if (response.IsSuccessStatusCode)
                {
                    return new MemberbaseResult
                    {
                        Success = true,
                        StatusCode = (int)response.StatusCode
                    };
                }

                var error = await response.Content.ReadAsStringAsync();

                return new MemberbaseResult
                {
                    Success = false,
                    StatusCode = (int)response.StatusCode,
                    ErrorMessage = error
                };
            }
            catch (Exception ex)
            {
                return new MemberbaseResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

    }
}
