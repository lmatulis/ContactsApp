using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace ContactsApp.Client.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/categories", category);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CategoryDTO>() ?? throw new HttpRequestException("Invalid JSON received from server");
        }

        public async Task DeleteCategoryAsync(int id, string userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/categories/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/categories/{categoryId}/email", emailData);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>("api/categories") ?? [];
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int Id, string userId)
        {
            return await _httpClient.GetFromJsonAsync<CategoryDTO>($"api/categories/{Id}");
        }

        public async Task UpdateCategoryAsync(CategoryDTO category, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/categories/{category.Id}", category);
            response.EnsureSuccessStatusCode();
        }
    }
}
