using FlowManager.Domain.Entities;
using System.Net.Http.Json;

namespace FlowManager.Client.Services
{
    public class StepService
    {
        private readonly HttpClient _httpClient;

        public StepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Step>> GetStepsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/steps");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<Step>>() ?? new List<Step>();
            }
            catch
            {
                return new List<Step>();
            }
        }

        public async Task<Step?> GetStepAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/steps/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Step>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Step>> GetStepsByFlowAsync(Guid flowId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/steps/flow/{flowId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Step>>() ?? new List<Step>();
                }
                return new List<Step>();
            }
            catch
            {
                return new List<Step>();
            }
        }

        public async Task<Step?> CreateStepAsync(Step step)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/steps", step);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Step>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateStepAsync(Guid id, Step step)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/steps/{id}", step);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteStepAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/steps/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AssignUserToStepAsync(Guid stepId, Guid userId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/steps/{stepId}/users/{userId}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UnassignUserFromStepAsync(Guid stepId, Guid userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/steps/{stepId}/users/{userId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddStepToFlowAsync(Guid stepId, Guid flowId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/steps/{stepId}/flows/{flowId}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveStepFromFlowAsync(Guid stepId, Guid flowId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/steps/{stepId}/flows/{flowId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}