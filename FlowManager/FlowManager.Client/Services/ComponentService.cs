using FlowManager.Domain.Entities;
using System.Net.Http.Json;

namespace FlowManager.Client.Services
{
    public class ComponentService
    {
        private readonly HttpClient _httpClient;

        public ComponentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Component>> GetAllComponentsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/components");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Component>>() ?? new List<Component>();
            }
            catch
            {
                return new List<Component>();
            }
        }

        public async Task<Component?> GetComponentByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/components/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Component>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Component?> CreateComponentAsync(Component component)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/components", component);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Component>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateComponentAsync(Guid id, Component component)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/components/{id}", component);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteComponentAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/components/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}