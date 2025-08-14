using FlowManager.Domain.Entities;
using System.Net.Http.Json;

namespace FlowManager.Client.Services
{
    public class FormService
    {
        private readonly HttpClient _httpClient;

        public FormService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FormResponse>> GetAllFormsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/formresponses");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<FormResponse>>() ?? new List<FormResponse>();
            }
            catch
            {
                return new List<FormResponse>();
            }
        }

        public async Task<FormResponse?> GetFormByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/formresponses/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FormResponse>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<FormResponse?> CreateFormAsync(FormResponse form)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/formresponses", form);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FormResponse>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateFormAsync(Guid id, FormResponse form)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/formresponses/{id}", form);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFormAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/formresponses/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<FormResponse>> GetFormsByUserAsync(Guid userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/formresponses/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<FormResponse>>() ?? new List<FormResponse>();
                }
                return new List<FormResponse>();
            }
            catch
            {
                return new List<FormResponse>();
            }
        }

        public async Task<List<FormResponse>> GetFormsByStepAsync(Guid stepId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/formresponses/step/{stepId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<FormResponse>>() ?? new List<FormResponse>();
                }
                return new List<FormResponse>();
            }
            catch
            {
                return new List<FormResponse>();
            }
        }

        public async Task<List<FormResponse>> GetFormsByTemplateAsync(Guid templateId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/formresponses/template/{templateId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<FormResponse>>() ?? new List<FormResponse>();
                }
                return new List<FormResponse>();
            }
            catch
            {
                return new List<FormResponse>();
            }
        }

        public async Task<bool> ApproveFormStepAsync(Guid formId, Guid moderatorId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/formresponses/{formId}/approve/{moderatorId}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RejectFormStepAsync(Guid formId, Guid moderatorId, string reason)
        {
            try
            {
                var payload = new { reason };
                var response = await _httpClient.PostAsJsonAsync($"api/formresponses/{formId}/reject/{moderatorId}", payload);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}