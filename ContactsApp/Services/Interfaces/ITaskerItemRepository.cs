using ContactsApp.Models;

namespace ContactsApp.Services.Interfaces
{
    public interface ITaskerItemRepository
    {
        Task<IEnumerable<TaskerItem>> GetTaskerItemsAsync(string userId);

        Task<TaskerItem> CreateTaskerItemAsync(TaskerItem taskerItem);

        Task<TaskerItem?> GetTaskerItemByIdAsync(Guid taskerItemId, string userId);

        Task UpdateTaskerItemAsync(TaskerItem taskerItem);

        Task DeleteTaskerItemByIdAsync(Guid taskerItemId, string userId);
    }
}
