using ContactsApp.Client.Models;

namespace ContactsApp.Client.Services.Interfaces
{
    public interface ICategoryService
    {
        //create
        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId);

        //get all categories
        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId);

        //delete Category
        Task DeleteCategoryAsync(int id, string userId);

        Task<CategoryDTO?> GetCategoryByIdAsync(int Id, string userId);

        Task UpdateCategoryAsync(CategoryDTO category, string userId);

        Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId);
    }
}
