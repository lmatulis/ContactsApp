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
    }
}
