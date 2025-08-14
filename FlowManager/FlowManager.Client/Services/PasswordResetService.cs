using System.Net.Http.Json;

namespace FlowManager.Client.Services
{
    public class PasswordResetService
    {
        private readonly HttpClient _httpClient;

        public PasswordResetService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendResetCodeAsync(string email)
        {
            try
            {
                var payload = new { email };
                var response = await _httpClient.PostAsJsonAsync("api/auth/send-reset-code", payload);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> VerifyResetCodeAsync(string email, string code)
        {
            try
            {
                var payload = new { email, code };
                var response = await _httpClient.PostAsJsonAsync("api/auth/verify-reset-code", payload);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email, string code, string newPassword)
        {
            try
            {
                var payload = new { email, code, newPassword };
                var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", payload);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}