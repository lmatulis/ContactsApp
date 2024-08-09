using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace ContactsApp.Client.Services
{

    public class TaskerItemService : ITaskerItemService
    {

        private readonly HttpClient _httpClient;


        public TaskerItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<TaskerItemDTO> CreateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/TaskerItem", taskerItem);
            response.EnsureSuccessStatusCode();

            TaskerItemDTO? taskerItemDTO = await response.Content.ReadFromJsonAsync<TaskerItemDTO>();
            return taskerItemDTO!;
        }

        public async Task DeleteTaskerItemByIdAsync(Guid taskerItemId, string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/TaskerItem/{taskerItemId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<TaskerItemDTO?> GetTaskerItemByIdAsync(Guid taskerItemId, string userId)
        {
            return await _httpClient.GetFromJsonAsync<TaskerItemDTO>($"api/TaskerItem/{taskerItemId}");
        }

        public async Task<IEnumerable<TaskerItemDTO>> GetTaskerItemsAsync(string userId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskerItemDTO>>("api/TaskerItem") ?? [];
        }

        public async Task UpdateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/TaskerItem/{taskerItem.Id}", taskerItem);
            response.EnsureSuccessStatusCode();
        }
    }
}