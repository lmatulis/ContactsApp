using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Models;
using ContactsApp.Services.Interfaces;

namespace ContactsApp.Services
{
    public class TaskerItemService : ITaskerItemService
    {


        private readonly ITaskerItemRepository _repository;




        public TaskerItemService(ITaskerItemRepository repository)
        {
            _repository = repository;
        }



        public async Task<TaskerItemDTO> CreateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            TaskerItem newTaskerItem = new TaskerItem()
            {
                Name = taskerItem.Name,
                IsComplete = taskerItem.IsComplete,
                UserId = userId,
            };
            TaskerItem createdTaskerItem = await _repository.CreateTaskerItemAsync(newTaskerItem);

            return createdTaskerItem.ToDTO();
        }


        public async Task DeleteTaskerItemByIdAsync(Guid taskerItemId, string userId)
        {
            await _repository.DeleteTaskerItemByIdAsync(taskerItemId, userId);
        }


        public async Task<TaskerItemDTO?> GetTaskerItemByIdAsync(Guid taskerItemId, string userId)
        {
            TaskerItem? taskerItem = await _repository.GetTaskerItemByIdAsync(taskerItemId, userId);
            return taskerItem?.ToDTO();
        }


        public async Task<IEnumerable<TaskerItemDTO>> GetTaskerItemsAsync(string userId)
        {
            IEnumerable<TaskerItem> taskerItems = await _repository.GetTaskerItemsAsync(userId);
            return taskerItems.Select(t => t.ToDTO());
        }


        public async Task UpdateTaskerItemAsync(TaskerItemDTO taskerItem, string userId)
        {
            TaskerItem? newtaskerItem = await _repository.GetTaskerItemByIdAsync(taskerItem.Id, userId);
            if (newtaskerItem != null)
            {
                newtaskerItem.Name = taskerItem.Name;
                newtaskerItem.IsComplete = taskerItem.IsComplete;

                await _repository.UpdateTaskerItemAsync(newtaskerItem);
            }
        }
    }
}
