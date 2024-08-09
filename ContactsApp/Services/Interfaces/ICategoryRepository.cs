using ContactsApp.Models;

namespace ContactsApp.Services.Interfaces
{
    public interface ICategoryRepository
    {
        //create
        Task<Category> CreateCategoryAsync(Category category);

        Task<Category?> GetCategoryByIdAsync(int Id, string userId);

        Task<List<Category>> GetCategoriesAsync(string userId);

        Task UpdateCategoryAsync(Category category, string userId);

        Task DeleteCategoryAsync(int id, string userId);
    }
}
