using FlowManager.Domain.Entities;
using System.Net.Http.Json;

namespace FlowManager.Client.Services
{
    public class FormTemplateService
    {
        private readonly HttpClient _httpClient;

        public FormTemplateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FormTemplate>> GetAllFormTemplatesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/formtemplates");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<FormTemplate>>() ?? new List<FormTemplate>();
            }
            catch
            {
                return new List<FormTemplate>();
            }
        }

        public async Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/formtemplates/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FormTemplate>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<FormTemplate?> CreateFormTemplateAsync(FormTemplate template)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/formtemplates", template);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<FormTemplate>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateFormTemplateAsync(Guid id, FormTemplate template)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/formtemplates/{id}", template);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteFormTemplateAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/formtemplates/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}