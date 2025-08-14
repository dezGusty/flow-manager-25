using FlowManager.Domain.Entities;
using System.Net.Http.Json;

namespace FlowManager.Client.Services
{
    public class RoleService
    {
        private readonly HttpClient _httpClient;

        public RoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/roles");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Role>>() ?? new List<Role>();
            }
            catch
            {
                return new List<Role>();
            }
        }

        public async Task<Role?> GetRoleByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/roles/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Role>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}