using FlowManager.Domain.Entities;
using System.Net.Http.Json;

namespace FlowManager.Client.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/users");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<User>>() ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<List<User>> GetAllModeratorsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/users/moderators");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<User>>() ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<List<User>> GetAllAdminsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/users/admins");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<User>>() ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<User>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/email/{email}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<User>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users", user);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<User>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(Guid id, User user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/users/{id}", user);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RestoreUserAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/users/{id}/restore", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(Guid id, string newPassword)
        {
            try
            {
                var payload = new { newPassword };
                var response = await _httpClient.PostAsJsonAsync($"api/users/{id}/reset-password", payload);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<User>> GetUsersByStepAsync(Guid stepId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/step/{stepId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<User>>() ?? new List<User>();
                }
                return new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }
    }
}