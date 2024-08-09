using ContactsApp.Client.Models;
using ContactsApp.Client.Services.Interfaces;
using ContactsApp.Models;
using ContactsApp.Services.Interfaces;

namespace ContactsApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        //constructor
        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        //create category
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                AppUserId = userId,
            };

            Category createdCategory = await _repository.CreateCategoryAsync(newCategory);
            return createdCategory.ToDTO();
        }

        public async Task DeleteCategoryAsync(int id, string userId)
        {
            await _repository.DeleteCategoryAsync(id, userId);
        }

        //get all categories
        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            IEnumerable<Category> categories = await _repository.GetCategoriesAsync(userId);
            //'select' acts like a foreach loop 
            IEnumerable<CategoryDTO> categoryDTOs = categories.Select(c => c.ToDTO());
            return categoryDTOs;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int Id, string userId)
        {
            Category? category = await _repository.GetCategoryByIdAsync(Id, userId);
            return category!.ToDTO();
        }
    }
}
